using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Player;

namespace ZaraEngine.HealthEngine
{
    public class FatigueHealthEffectsController
    {

        private const float MaxHeartRateBonus     = 10f; // bpm
        private const float MaxBloodPressureBonus = 15f; // mmHg

        private readonly IGameController _gc;
        private readonly HealthController _healthController;

        public float BloodPressureTopBonus { get; private set; }
        public float BloodPressureBottomBonus { get; private set; }
        public float HeartRateBonus { get; private set; }

        public FatigueHealthEffectsController(IGameController gc, HealthController health)
        {
            _gc = gc;
            _healthController = health;
        }

        public void SlowUpdate()
        {
            ProcessFatigueEffects();
        }

        private void ProcessFatigueEffects()
        {
            var fatiguePerc = _healthController.Status.FatiguePercentage / 100f;

            if (fatiguePerc > 1f)
                fatiguePerc = 1f;

            BloodPressureTopBonus = -Helpers.Lerp(0f, MaxBloodPressureBonus, fatiguePerc);
            BloodPressureBottomBonus = -Helpers.Lerp(0f, MaxBloodPressureBonus, fatiguePerc);
            HeartRateBonus = -Helpers.Lerp(0f, MaxHeartRateBonus, fatiguePerc);
        }

    }
}
