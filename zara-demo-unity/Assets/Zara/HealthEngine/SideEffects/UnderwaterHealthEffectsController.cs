using System;
using ZaraEngine.StateManaging;

namespace ZaraEngine.HealthEngine
{
    public class UnderwaterHealthEffectsController : IAcceptsStateChange
    {


        /* ------------------------------------------------------------------------------------------------------------------ *
         * ---------------------------------------------------- Underwater -------------------------------------------------- *
         * ------------------------------------------------------------------------------------------------------------------ */

        public const float UnderwaterOxygenDrainRate          = 0.16f; // percents per game second
        private const float MaxHeartRateBonus                 = 41f;   // bpm
        private const float MaxBloodPressureBonus             = 26f;   // mmHg
        private const float MaxOxygenBonus                    = 100f;  // percents
        private const float MaxGameMinutesUnderwater          = 6.5f;  // game minutes

        public float BloodPressureTopBonus { get; private set; }
        public float BloodPressureBottomBonus { get; private set; }
        public float HeartRateBonus { get; private set; }
        public float OxygenLevelBonus { get; private set; }

        private bool _lastUnderWaterState;

        private DateTime? _gameTimeGotUnderwater;

        private readonly IGameController _gc;
        private readonly HealthController _healthController;

        private readonly FixedEvent _drowningDeathEvent;

        public UnderwaterHealthEffectsController(IGameController gc, HealthController health)
        {
            _gc = gc;
            _healthController = health;

            // Game events produced by the Underwater Effects Controller

            _drowningDeathEvent = new FixedEvent("Drowning death", ev => Events.NotifyAll(l => l.DeathByDrowning())) { AutoReset = true };
        }

        public void Update(float gameSecondsSinceLastCall, float deltaTime)
        {
            ProcessUnderwaterEffects(gameSecondsSinceLastCall, deltaTime);
        }

        private void ProcessUnderwaterEffects(float gameSecondsSinceLastCall, float deltaTime)
        {
            if (_lastUnderWaterState && !_gc.Player.IsUnderWater)
            {
                _lastUnderWaterState = false;
            }

            if (!_lastUnderWaterState && _gc.Player.IsUnderWater)
            {
                _lastUnderWaterState = true;
                _gameTimeGotUnderwater = _gc.WorldTime.Value.AddMilliseconds(-1); // to not trigger the if on a first iteration
            }

            if (!_lastUnderWaterState && _gameTimeGotUnderwater.HasValue)
            {
                _gameTimeGotUnderwater = _gameTimeGotUnderwater.Value.AddSeconds(gameSecondsSinceLastCall*2); // for lerping back
            }

            CalculateVitalsBonus(deltaTime);
        }

        private void CalculateVitalsBonus(float deltaTime)
        {
            if (_gc.Health.Status.OxygenPercentage <= 0f)
            {
                _drowningDeathEvent.Invoke(deltaTime);

                return;
            }

            if (_gameTimeGotUnderwater.HasValue)
            {
                if(_gameTimeGotUnderwater.Value >= _gc.WorldTime.Value)
                {
                    _gameTimeGotUnderwater = null;

                    return;
                }

                var gameMinutesUnterwater = (_gc.WorldTime.Value - _gameTimeGotUnderwater.Value).TotalMinutes;
                var oxyPercent = (float)gameMinutesUnterwater / (float)MaxGameMinutesUnderwater;

                BloodPressureTopBonus = Helpers.Lerp(0f, MaxBloodPressureBonus, oxyPercent);
                BloodPressureBottomBonus = Helpers.Lerp(0f, MaxBloodPressureBonus, oxyPercent);
                HeartRateBonus = Helpers.Lerp(0f, MaxHeartRateBonus, oxyPercent);
                OxygenLevelBonus = -Helpers.Lerp(0f, MaxOxygenBonus, oxyPercent);
            }
        }

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new UnderwaterHealthEffectsSnippet
            {
                BloodPressureBottomBonus = this.BloodPressureBottomBonus,
                BloodPressureTopBonus = this.BloodPressureTopBonus,
                HeartRateBonus = this.HeartRateBonus,
                OxygenLevelBonus = this.OxygenLevelBonus,
                LastUnderWaterState = _lastUnderWaterState,
                GameTimeGotUnderwater = _gameTimeGotUnderwater
            };

            state.ChildStates.Add("DrowningDeathEvent", _drowningDeathEvent.GetState());

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (UnderwaterHealthEffectsSnippet)savedState;

            BloodPressureBottomBonus = state.BloodPressureBottomBonus;
            BloodPressureTopBonus = state.BloodPressureTopBonus;
            HeartRateBonus = state.HeartRateBonus;
            OxygenLevelBonus = state.OxygenLevelBonus;

            _lastUnderWaterState = state.LastUnderWaterState;
            _gameTimeGotUnderwater = state.GameTimeGotUnderwater;

            _drowningDeathEvent.RestoreState(state.ChildStates["DrowningDeathEvent"]);
        }

        #endregion 

    }
}
