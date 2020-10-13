using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Player;
using UnityEngine;

namespace ZaraEngine.HealthEngine
{
    public class UnderwaterHealthEffectsController
    {


        /* ------------------------------------------------------------------------------------------------------------------ *
         * ---------------------------------------------------- Underwater -------------------------------------------------- *
         * ------------------------------------------------------------------------------------------------------------------ */

        public const float UnderwaterOxygenDrainRate         = 0.16f; // percents per game second
        private const float MaxHeartRateBonus                 = 24f;   // bpm
        private const float MaxBloodPressureBonus             = 11f;   // mmHg
        private const float MaxFatigueImpactOnOxygenConsuming = 0.04f; // percents
        private const float OxygenLevelRestoreRate            = 4f;

        public float BloodPressureTopBonus { get; private set; }
        public float BloodPressureBottomBonus { get; private set; }
        public float HeartRateBonus { get; private set; }

        public float OxygenLevel = 100f;

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

        public void Update(float gameSecondsSinceLastCall)
        {
            ProcessUnderwaterEffects(gameSecondsSinceLastCall);
        }

        private void ProcessUnderwaterEffects(float gameSecondsSinceLastCall)
        {
            if (!_lastUnderWaterState && !_gc.Player.IsUnderWater)
            {
                if (Math.Abs(OxygenLevel - 100f) > 0.00001f)
                {
                    // Restore oxygen level
                    OxygenLevel += UnderwaterOxygenDrainRate * gameSecondsSinceLastCall * OxygenLevelRestoreRate;

                    if (OxygenLevel > 100f)
                        OxygenLevel = 100f;

                    CalculateVitalsBonus();
                }
            }
            else
            {
                // Drain oxygen level
                OxygenLevel -= UnderwaterOxygenDrainRate * gameSecondsSinceLastCall + MaxFatigueImpactOnOxygenConsuming * (_healthController.Status.FatiguePercentage / 100f);
                CalculateVitalsBonus();
            }

            if (_lastUnderWaterState && !_gc.Player.IsUnderWater)
            {
                //if (OxygenLevel < 20f)
                //    _playHardBreath.Invoke();
                //else if (OxygenLevel < 40f)
                //    _playMediumBreath.Invoke();
                //else if (OxygenLevel < 70f)
                //    _playLightBreath.Invoke();

                _lastUnderWaterState = _gc.Player.IsUnderWater;
            }

            if (!_lastUnderWaterState && _gc.Player.IsUnderWater)
            {
                _lastUnderWaterState = _gc.Player.IsUnderWater;
            }            
        }

        private void CalculateVitalsBonus()
        {
            if (OxygenLevel <= 0f)
            {
                _drowningDeathEvent.Invoke();

                return;
            }

            var oxyPercent = 1 - OxygenLevel / 100f;

            BloodPressureTopBonus = Mathf.Lerp(0f, MaxBloodPressureBonus, oxyPercent);
            BloodPressureBottomBonus = Mathf.Lerp(0f, MaxBloodPressureBonus, oxyPercent);
            HeartRateBonus = Mathf.Lerp(0f, MaxHeartRateBonus, oxyPercent);
        }

    }
}
