using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class UnderwaterHealthEffectsContract
    {

        public float BloodPressureTopBonus;
        public float BloodPressureBottomBonus;
        public float HeartRateBonus;
        public float OxygenLevelBonus;
        public bool LastUnderWaterState;
        public DateTimeContract GameTimeGotUnderwater;

        public FixedEventContract DrowningDeathEvent;

    }
}
