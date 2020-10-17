using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Player;
using ZaraEngine.StateManaging;

namespace ZaraEngine.HealthEngine
{
    public class UnderwaterHealthEffectsController : IAcceptsStateChange
    {


        /* ------------------------------------------------------------------------------------------------------------------ *
         * ---------------------------------------------------- Underwater -------------------------------------------------- *
         * ------------------------------------------------------------------------------------------------------------------ */

        public const float UnderwaterOxygenDrainRate         = 0.16f; // percents per game second
        private const float MaxHeartRateBonus                 = 24f;   // bpm
        private const float MaxBloodPressureBonus             = 11f;   // mmHg
        private const float MaxFatigueImpactOnOxygenConsuming = 0.04f; // percents
        private const float MaxOxygenBonus                    = 100f;  // percents

        public float BloodPressureTopBonus { get; private set; }
        public float BloodPressureBottomBonus { get; private set; }
        public float HeartRateBonus { get; private set; }
        public float OxygenLevelBonus { get; private set; }

        private bool _lastUnderWaterState;

        private readonly IGameController _gc;
        private readonly HealthController _healthController;

        private readonly FixedEvent _drowningDeathEvent;
        private readonly FixedEvent _playLightBreath;
        private readonly FixedEvent _playMediumBreath;
        private readonly FixedEvent _playHardBreath;

        public UnderwaterHealthEffectsController(IGameController gc, HealthController health)
        {
            _gc = gc;
            _healthController = health;

            // Game events produced by the Underwater Effects Controller

            _drowningDeathEvent = new FixedEvent("Drowning death",          ev => Events.NotifyAll(l => l.DeathByDrowning())) { AutoReset = true };
            _playLightBreath    = new FixedEvent("Off-water light breath",  ev => Events.NotifyAll(l => l.SwimmingOffWaterLightBreath())) { AutoReset = true };
            _playMediumBreath   = new FixedEvent("Off-water meduim breath", ev => Events.NotifyAll(l => l.SwimmingOffWaterMediumBreath())) { AutoReset = true };
            _playHardBreath     = new FixedEvent("Off-water hard breath",   ev => Events.NotifyAll(l => l.SwimmingOffWaterHardBreath())) { AutoReset = true };
        }

        public void Update(float gameSecondsSinceLastCall, float deltaTime)
        {
            ProcessUnderwaterEffects(gameSecondsSinceLastCall, deltaTime);
        }

        private void ProcessUnderwaterEffects(float gameSecondsSinceLastCall, float deltaTime)
        {
            if (_lastUnderWaterState && !_gc.Player.IsUnderWater)
            {
                if (_gc.Health.Status.OxygenPercentage < 20f)
                    _playHardBreath.Invoke(deltaTime);
                else if (_gc.Health.Status.OxygenPercentage < 40f)
                    _playMediumBreath.Invoke(deltaTime);
                else if (_gc.Health.Status.OxygenPercentage < 70f)
                    _playLightBreath.Invoke(deltaTime);

                _lastUnderWaterState = _gc.Player.IsUnderWater;
            }

            if (!_lastUnderWaterState && _gc.Player.IsUnderWater)
            {
                _lastUnderWaterState = _gc.Player.IsUnderWater;
            }            
        }

        private void CalculateVitalsBonus(float deltaTime)
        {
            if (_gc.Health.Status.OxygenPercentage <= 0f)
            {
                _drowningDeathEvent.Invoke(deltaTime);

                return;
            }

            var oxyPercent = 1f - _gc.Health.Status.OxygenPercentage / 100f;

            BloodPressureTopBonus = Helpers.Lerp(0f, MaxBloodPressureBonus, oxyPercent);
            BloodPressureBottomBonus = Helpers.Lerp(0f, MaxBloodPressureBonus, oxyPercent);
            HeartRateBonus = Helpers.Lerp(0f, MaxHeartRateBonus, oxyPercent);
            OxygenLevelBonus = -Helpers.Lerp(0f, MaxOxygenBonus, oxyPercent);
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
                LastUnderWaterState = _lastUnderWaterState
            };

            state.ChildStates.Add("DrowningDeathEvent", _drowningDeathEvent.GetState());
            state.ChildStates.Add("PlayLightBreath", _playLightBreath.GetState());
            state.ChildStates.Add("PlayMediumBreath", _playMediumBreath.GetState());
            state.ChildStates.Add("PlayHardBreath", _playHardBreath.GetState());

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

            _drowningDeathEvent.RestoreState(state.ChildStates["DrowningDeathEvent"]);
            _playLightBreath.RestoreState(state.ChildStates["PlayLightBreath"]);
            _playMediumBreath.RestoreState(state.ChildStates["PlayMediumBreath"]);
            _playHardBreath.RestoreState(state.ChildStates["PlayHardBreath"]);
        }

        #endregion 

    }
}
