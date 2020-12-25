using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class ClothesHealthEffectsContract
    {

        public DateTimeContract LastClothesChangeTime;
        public DateTimeContract LastAutoReLerpTime;
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
