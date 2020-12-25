using System;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Diseases
{
    public class HypothermiaMonitor : DiseaseMonitorBase, IAcceptsStateChange
    {
        private const int MonitorCheckInterval              = 7;     // Game minutes
        private const float HypothermiaWarmthLevelThreshold = -20f;  // Warmth Score
        private const int HypothermiaChance                 = 44;    // Percent

        private const int ChanceToDieInSleepForWorryingStage  = 30;  // Percent
        private const int ChanceToDieInSleepForCriticalStage  = 93;  // Percent
        private const int ChanceToDieForCriticalStage         = 70;  // Percent

        private DateTime? _nextCheckTime;
        private bool _isDiseaseActivated;
        private float _currentHypothermiaWarmthLevelThreshold;

        private EventByChance _hypothermiaDeathEvent;

        public HypothermiaMonitor(IGameController gc) : base(gc) 
        {
            // Events produced by the Hypothermia Monitor
            _hypothermiaDeathEvent = new EventByChance("Hypothermia death", ev => Events.NotifyAll(l => l.HypothermiaDeath()), 0 /*will be set in Check()*/, ZaraEngine.HealthEngine.HealthController.HealthUpdateInterval /*will roll the dice on every Check() call */, ZaraEngine.HealthEngine.HealthController.HealthUpdateInterval) { AutoReset = true };
        }

        public override void Check(float deltaTime)
        {
            if (!_gc.WorldTime.HasValue)
                return;

            if (_gc.Body.IsSleeping)
            {
                var sleepyHypothermia = _gc.Health.Status.GetActiveOrScheduled<Hypothermia>(_gc.WorldTime.Value) as ActiveDisease;

                if(sleepyHypothermia != null)
                {
                    var sleepyHypothermiaState = sleepyHypothermia.GetActiveStage(_gc.WorldTime.Value);

                    if(sleepyHypothermiaState != null)
                    {
                        if(sleepyHypothermiaState.Level == DiseaseLevels.Worrying)
                        {
                            if(sleepyHypothermia.TreatedStage == null)
                                _hypothermiaDeathEvent.Check(ChanceToDieInSleepForWorryingStage, deltaTime);
                        }
                        if (sleepyHypothermiaState.Level == DiseaseLevels.Critical)
                        {
                            if(sleepyHypothermia.TreatedStage == null)
                                _hypothermiaDeathEvent.Check(ChanceToDieInSleepForCriticalStage, deltaTime);
                        }
                    }
                }
            }

            if (_nextCheckTime.HasValue)
            {
                if (_gc.WorldTime.Value >= _nextCheckTime.Value)
                {
                    _nextCheckTime = null;
                }
                else return;
            }
            else
            {
                _nextCheckTime = _gc.WorldTime.Value.AddMinutes(_isDiseaseActivated ? MonitorCheckInterval/5f : MonitorCheckInterval);

                return;
            }

            var activeDisease = _gc.Health.Status.GetActiveOrScheduled<Hypothermia>(_gc.WorldTime.Value) as ActiveDisease;

            if (activeDisease == null)
            {
                _currentHypothermiaWarmthLevelThreshold = 0f;
                _isDiseaseActivated = false;
            }
            else
            {
                var stage = activeDisease.GetActiveStage(_gc.WorldTime.Value);

                if (stage != null && stage.Level == DiseaseLevels.Critical)
                {
                    if(activeDisease.TreatedStage == null)
                        _hypothermiaDeathEvent.Check(ChanceToDieForCriticalStage, deltaTime);
                }
            }

            void activateDisease()
            {
                // Activate the disease

                if (activeDisease != null)
                {
                    // Resume hypothermia that currently is being healed

                    activeDisease.InvertBack();
                    return;
                }

                _nextCheckTime = _gc.WorldTime.Value.AddMinutes(MonitorCheckInterval);
                _gc.Health.Status.ActiveDiseases.Add(new ActiveDisease(_gc, typeof(Hypothermia), _gc.WorldTime.Value));

                _isDiseaseActivated = true;
            }

            // Extreme cold case
            if (_gc.Body.WarmthLevelCached <= HypothermiaWarmthLevelThreshold - 10)
            {
                // Right away
                activateDisease();

                return;
            }

            if(Math.Abs(_currentHypothermiaWarmthLevelThreshold) <= 0.0000001f)
                _currentHypothermiaWarmthLevelThreshold = HypothermiaWarmthLevelThreshold;

            if (_gc.Body.WarmthLevelCached <= _currentHypothermiaWarmthLevelThreshold)
            {
                if (activeDisease != null)
                {
                    var stage = activeDisease.GetActiveStage(_gc.WorldTime.Value);

                    if (stage != null)
                    {
                        if (stage.Level == DiseaseLevels.InitialStage)
                        {
                            // we're cold again
                            if (_gc.Body.WarmthLevelCached <= HypothermiaWarmthLevelThreshold + 6f)
                            {
                                _currentHypothermiaWarmthLevelThreshold = HypothermiaWarmthLevelThreshold + 6f;

                                activeDisease.InvertBack();
                            }
                        }

                        if (stage.Level == DiseaseLevels.Worrying)
                        {
                            // we're cold again
                            if (_gc.Body.WarmthLevelCached <= HypothermiaWarmthLevelThreshold + 12f)
                            {
                                _currentHypothermiaWarmthLevelThreshold = HypothermiaWarmthLevelThreshold + 12f;

                                activeDisease.InvertBack();
                            }
                        }

                        if (stage.Level == DiseaseLevels.Critical)
                        {
                            // we're cold again
                            if (_gc.Body.WarmthLevelCached <= HypothermiaWarmthLevelThreshold + 15f)
                            {
                                _currentHypothermiaWarmthLevelThreshold = HypothermiaWarmthLevelThreshold + 15f;

                                activeDisease.InvertBack();
                            }
                        }
                    }

                    return;
                }
                if (HypothermiaChance.WillHappen())
                {
                    activateDisease();
                }
            }
            else
            {
                // Getting warmer
                if(activeDisease != null)
                {
                    var stage = activeDisease.GetActiveStage(_gc.WorldTime.Value);

                    if (stage != null)
                    {
                        if (stage.Level == DiseaseLevels.InitialStage)
                        {
                            // in order for first stage to start healing, we need
                            // to be just a little warmer
                            if (_gc.Body.WarmthLevelCached >= HypothermiaWarmthLevelThreshold + 6f)
                            {
                                _currentHypothermiaWarmthLevelThreshold = HypothermiaWarmthLevelThreshold + 6f;

                                activeDisease.Invert();
                            }
                        }

                        if (stage.Level == DiseaseLevels.Worrying)
                        {
                            // in order for second stage to start healing, we need
                            // to be considerably warmer
                            if (_gc.Body.WarmthLevelCached >= HypothermiaWarmthLevelThreshold + 12f)
                            {
                                _currentHypothermiaWarmthLevelThreshold = HypothermiaWarmthLevelThreshold + 12f;

                                activeDisease.Invert();
                            }
                        }

                        if (stage.Level == DiseaseLevels.Critical)
                        {
                            // in order for last stage to start healing, we need
                            // to be very warm
                            if (_gc.Body.WarmthLevelCached >= HypothermiaWarmthLevelThreshold + 15f)
                            {
                                _currentHypothermiaWarmthLevelThreshold = HypothermiaWarmthLevelThreshold + 15f;

                                activeDisease.Invert();
                            }
                        }
                    }
                }
            }
        }

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new HypothermiaMonitorSnippet
            {
                NextCheckTime = _nextCheckTime,
                IsDiseaseActivated = _isDiseaseActivated,
                CurrentHypothermiaWarmthLevelThreshold = _currentHypothermiaWarmthLevelThreshold
            };

            state.ChildStates.Add("HypothermiaDeathEvent", _hypothermiaDeathEvent.GetState());

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (HypothermiaMonitorSnippet)savedState;

            _nextCheckTime = state.NextCheckTime;
            _isDiseaseActivated = state.IsDiseaseActivated;
            _currentHypothermiaWarmthLevelThreshold = state.CurrentHypothermiaWarmthLevelThreshold;

            _hypothermiaDeathEvent.RestoreState(state.ChildStates["HypothermiaDeathEvent"]);
        }

        #endregion 

    }
}