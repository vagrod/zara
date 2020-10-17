using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class ClothesHealthEffectsContract
    {

        public DateTime? LastClothesChangeTime;
        public DateTime? LastAutoReLerpTime;
        public float TargetBodyTemperatureDelta;
        public float TargetHeartRateDelta;
        public float CurrentTemperatureBonus;
        public float CurrentHeartRateBonus;
        public float PlayerRunSpeedBonus;
        public float HeartRateBonus;
        public float BodyTemperatureBonus;
        public float StaminaBonus;

    }
}
