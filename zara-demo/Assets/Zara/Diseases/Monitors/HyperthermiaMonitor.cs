using System;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Diseases
{
    public class HyperthermiaMonitor : DiseaseMonitorBase, IAcceptsStateChange
    {

        private const int MonitorCheckInterval               = 7;     // Game minutes
        private const float HyperthermiaWarmthLevelThreshold = 24f;   // Warmth Score
        private const int HyperthermiaChance                 = 62;    // Percent

        private const int ChanceToDieInSleepForWorryingStage  = 20;  // Percent
        private const int ChanceToDieInSleepForCriticalStage  = 83;  // Percent
        private const int ChanceToDieForCriticalStage         = 71;  // Percent

        private DateTime? _nextCheckTime;
        private bool _isDiseaseActivated;
        private float _currentHyperthermiaWarmthLevelThreshold;

        private EventByChance _hyperthermiaDeathEvent;

        public HyperthermiaMonitor(IGameController gc) : base(gc) 
        {
            // Events produced by the Hyperthermia Monitor
            _hyperthermiaDeathEvent = new EventByChance("Hyperthermia death", ev => Events.NotifyAll(l => l.HyperthermiaDeath()), 0 /*will be set in Check()*/, ZaraEngine.HealthEngine.HealthController.HealthUpdateInterval /*will roll the dice on every Check() call */, ZaraEngine.HealthEngine.HealthController.HealthUpdateInterval) { AutoReset = true };
        }

        public override void Check(float deltaTime)
        {
            if (!_gc.WorldTime.HasValue)
                return;

            if (_gc.Body.IsSleeping)
            {
                var sleepyHyperthermia = _gc.Health.Status.GetActiveOrScheduled<Hyperthermia>(_gc.WorldTime.Value) as ActiveDisease;

                if(sleepyHyperthermia != null)
                {
                    var sleepyHyperthermiaState = sleepyHyperthermia.GetActiveStage(_gc.WorldTime.Value);

                    if(sleepyHyperthermiaState != null)
                    {
                        if(sleepyHyperthermiaState.Level == DiseaseLevels.Worrying)
                        {
                            if(sleepyHyperthermia.TreatedStage == null)
                                _hyperthermiaDeathEvent.Check(ChanceToDieInSleepForWorryingStage, deltaTime);
                        }
                        if (sleepyHyperthermiaState.Level == DiseaseLevels.Critical)
                        {
                            if(sleepyHyperthermia.TreatedStage == null)
                                _hyperthermiaDeathEvent.Check(ChanceToDieInSleepForCriticalStage, deltaTime);
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

            var activeDisease = _gc.Health.Status.GetActiveOrScheduled<Hyperthermia>(_gc.WorldTime.Value) as ActiveDisease;

            if (activeDisease == null)
            {
                _currentHyperthermiaWarmthLevelThreshold = 0f;
                _isDiseaseActivated = false;
            }
            else
            {
                var stage = activeDisease.GetActiveStage(_gc.WorldTime.Value);

                if (stage != null && stage.Level == DiseaseLevels.Critical)
                {
                    if(activeDisease.TreatedStage == null)
                        _hyperthermiaDeathEvent.Check(ChanceToDieForCriticalStage, deltaTime);
                }
            }

            void activateDisease()
            {
                // Activate the disease

                if (activeDisease != null)
                {
                    // Resume Hyperthermia that currently is being healed

                    activeDisease.InvertBack();
                    return;
                }

                _nextCheckTime = _gc.WorldTime.Value.AddMinutes(MonitorCheckInterval);
                _gc.Health.Status.ActiveDiseases.Add(new ActiveDisease(_gc, typeof(Hyperthermia), _gc.WorldTime.Value));

                _isDiseaseActivated = true;
            }

            // Extreme heat case
            if (_gc.Body.WarmthLevelCached >= HyperthermiaWarmthLevelThreshold + 10)
            {
                // Right away
                activateDisease();

                return;
            }

            if(Math.Abs(_currentHyperthermiaWarmthLevelThreshold) <= 0.0000001f)
                _currentHyperthermiaWarmthLevelThreshold = HyperthermiaWarmthLevelThreshold;

            if (_gc.Body.WarmthLevelCached >= _currentHyperthermiaWarmthLevelThreshold)
            {
                if (activeDisease != null)
                {
                    var stage = activeDisease.GetActiveStage(_gc.WorldTime.Value);

                    if (stage != null)
                    {
                        if (stage.Level == DiseaseLevels.InitialStage)
                        {
                            // we're hot again
                            if (_gc.Body.WarmthLevelCached >= HyperthermiaWarmthLevelThreshold - 6f)
                            {
                                _currentHyperthermiaWarmthLevelThreshold = HyperthermiaWarmthLevelThreshold - 6f;

                                activeDisease.InvertBack();
                            }
                        }

                        if (stage.Level == DiseaseLevels.Worrying)
                        {
                            // we're hot again
                            if (_gc.Body.WarmthLevelCached >= HyperthermiaWarmthLevelThreshold - 12f)
                            {
                                _currentHyperthermiaWarmthLevelThreshold = HyperthermiaWarmthLevelThreshold - 12f;

                                activeDisease.InvertBack();
                            }
                        }

                        if (stage.Level == DiseaseLevels.Critical)
                        {
                            // we're hot again
                            if (_gc.Body.WarmthLevelCached >= HyperthermiaWarmthLevelThreshold - 15f)
                            {
                                _currentHyperthermiaWarmthLevelThreshold = HyperthermiaWarmthLevelThreshold - 15f;

                                activeDisease.InvertBack();
                            }
                        }
                    }

                    return;
                }
                if (HyperthermiaChance.WillHappen())
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
                            // to be just a little colder
                            if (_gc.Body.WarmthLevelCached <= HyperthermiaWarmthLevelThreshold - 6f)
                            {
                                _currentHyperthermiaWarmthLevelThreshold = HyperthermiaWarmthLevelThreshold - 6f;

                                activeDisease.Invert();
                            }
                        }

                        if (stage.Level == DiseaseLevels.Worrying)
                        {
                            // in order for second stage to start healing, we need
                            // to be considerably colder
                            if (_gc.Body.WarmthLevelCached <= HyperthermiaWarmthLevelThreshold - 12f)
                            {
                                _currentHyperthermiaWarmthLevelThreshold = HyperthermiaWarmthLevelThreshold - 12f;

                                activeDisease.Invert();
                            }
                        }

                        if (stage.Level == DiseaseLevels.Critical)
                        {
                            // in order for last stage to start healing, we need
                            // to be very cold
                            if (_gc.Body.WarmthLevelCached <= HyperthermiaWarmthLevelThreshold - 15f)
                            {
                                _currentHyperthermiaWarmthLevelThreshold = HyperthermiaWarmthLevelThreshold - 15f;

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
            var state = new HyperthermiaMonitorSnippet
            {
                NextCheckTime = _nextCheckTime,
                IsDiseaseActivated = _isDiseaseActivated,
                CurrentHyperthermiaWarmthLevelThreshold = _currentHyperthermiaWarmthLevelThreshold
            };

            state.ChildStates.Add("HyperthermiaDeathEvent", _hyperthermiaDeathEvent.GetState());

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (HyperthermiaMonitorSnippet)savedState;

            _nextCheckTime = state.NextCheckTime;
            _isDiseaseActivated = state.IsDiseaseActivated;
            _currentHyperthermiaWarmthLevelThreshold = state.CurrentHyperthermiaWarmthLevelThreshold;

            _hyperthermiaDeathEvent.RestoreState(state.ChildStates["HyperthermiaDeathEvent"]);
        }

        #endregion 

    }
}
