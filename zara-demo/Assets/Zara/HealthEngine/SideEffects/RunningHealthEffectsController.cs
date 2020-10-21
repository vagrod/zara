using System;
using ZaraEngine.StateManaging;

namespace ZaraEngine.HealthEngine
{
    public class RunningHealthEffectsController : IAcceptsStateChange
    {

        /* ------------------------------------------------------------------------------------------------------------------ *
         * ----------------------------------------------------- Running ---------------------------------------------------- *
         * ------------------------------------------------------------------------------------------------------------------ */
        private const int SecondsUntilMaxRunningDeltaReached = 320;  // Game seconds
        private const int SecondsOfRunningBeforeWheeze       = 260;  // Game seconds
        private const float MaxHeartRateBonus                = 24f;  // bpm
        private const float MaxBloodPressureBonus            = 11f;  // mmHg
        private const float MaxBodyTemperatureBonus          = 0.5f; // C
        private const float MaxOxygenBonus                   = 16f;  // percent
        private const float RunningOxygenDrainRate            = 0.05f; // percents per game second
        private const float RunningOxygenThreshold            = 84f;   // percents

        private readonly IGameController _gc;
        private readonly HealthController _healthController;

        private readonly FixedEvent _intenseRunningOnEvent;
        private readonly FixedEvent _intenseRunningOffEvent;

        public float BloodPressureTopBonus { get; private set; }
        public float BloodPressureBottomBonus { get; private set; }
        public float HeartRateBonus { get; private set; }
        public float BodyTemperatureBonus { get; private set; }
        public float OxygenLevelBonus { get; private set; }

        private float _gameSecondsInRunningState;
        private bool _isWheezeEventTriggered;

        public RunningHealthEffectsController(IGameController gc, HealthController health)
        {
            _gc = gc;
            _healthController = health;

            // Game events produced by Running Effects Controller

            _intenseRunningOnEvent  = new FixedEvent("Intence running on trigger",  ev => Events.NotifyAll(l => l.IntenseRunningTriggeredOn())) { AutoReset = true };
            _intenseRunningOffEvent = new FixedEvent("Intence running off trigger", ev => Events.NotifyAll(l => l.IntenseRunningTriggeredOff())) { AutoReset = true };
        }

        public void Update(float gameSecondsSinceLastCall, float deltaTime)
        {
            ProcessRunningEffects(gameSecondsSinceLastCall, deltaTime);
        }

        private void ProcessRunningEffects(float gameSecondsSinceLastCall, float deltaTime)
        {
            if (_healthController.UnconsciousMode)
                return;

            if (_gc.Player.IsRunning && !_gc.Health.Status.CannotRun && !_gc.Player.IsUnderWater && !_gc.Player.IsSwimming && !_healthController.Status.IsLegFracture && !_healthController.IsInventoryOverload)
                _gameSecondsInRunningState += gameSecondsSinceLastCall;
            else
                _gameSecondsInRunningState -= gameSecondsSinceLastCall;

            if (_gameSecondsInRunningState < 0.00001f)
            {
                _gameSecondsInRunningState = 0;
            }

            if (_gameSecondsInRunningState >= SecondsUntilMaxRunningDeltaReached)
            {
                _gameSecondsInRunningState = SecondsUntilMaxRunningDeltaReached;
            }

            if (Math.Abs(_gameSecondsInRunningState) < 0.00001f)
                return;

            var runningPerc = _gameSecondsInRunningState / SecondsUntilMaxRunningDeltaReached;

            BloodPressureTopBonus = Helpers.Lerp(0f, MaxBloodPressureBonus, runningPerc);
            BloodPressureBottomBonus = Helpers.Lerp(0f, MaxBloodPressureBonus, runningPerc);
            HeartRateBonus = Helpers.Lerp(0f, MaxHeartRateBonus, runningPerc);
            BodyTemperatureBonus = Helpers.Lerp(0f, MaxBodyTemperatureBonus, runningPerc);
            OxygenLevelBonus = _gc.Player.IsUnderWater || _gc.Body.IsSleeping ? 0f : -Helpers.Lerp(0f, MaxOxygenBonus, runningPerc); // ignore if underwater

            if (_gameSecondsInRunningState > SecondsOfRunningBeforeWheeze)
            {
                if (!_isWheezeEventTriggered)
                {
                    _intenseRunningOnEvent.Invoke(deltaTime);
                    _isWheezeEventTriggered = true;
                }
            }
            else
            {
                if (_gameSecondsInRunningState < SecondsOfRunningBeforeWheeze)
                {
                    if (_isWheezeEventTriggered)
                    {
                        _intenseRunningOffEvent.Invoke(deltaTime);
                        _isWheezeEventTriggered = false;
                    }
                }
            }
        }

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new RunningHealthEffectsSnippet
            {
                BloodPressureBottomBonus = this.BloodPressureBottomBonus,
                BloodPressureTopBonus = this.BloodPressureTopBonus,
                HeartRateBonus = this.HeartRateBonus,
                OxygenLevelBonus = this.OxygenLevelBonus,
                BodyTemperatureBonus = this.BodyTemperatureBonus,
                GameSecondsInRunningState = _gameSecondsInRunningState,
                IsWheezeEventTriggered = _isWheezeEventTriggered
            };

            state.ChildStates.Add("IntenseRunningOnEvent", _intenseRunningOnEvent.GetState());
            state.ChildStates.Add("IntenseRunningOffEvent", _intenseRunningOffEvent.GetState());

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (RunningHealthEffectsSnippet)savedState;

            BloodPressureBottomBonus = state.BloodPressureBottomBonus;
            BloodPressureTopBonus = state.BloodPressureTopBonus;
            HeartRateBonus = state.HeartRateBonus;
            OxygenLevelBonus = state.OxygenLevelBonus;
            BodyTemperatureBonus = state.BodyTemperatureBonus;

            _gameSecondsInRunningState = state.GameSecondsInRunningState;
            _isWheezeEventTriggered = state.IsWheezeEventTriggered;

            _intenseRunningOnEvent.RestoreState(state.ChildStates["IntenseRunningOnEvent"]);
            _intenseRunningOffEvent.RestoreState(state.ChildStates["IntenseRunningOffEvent"]);
        }

        #endregion 

    }
}
