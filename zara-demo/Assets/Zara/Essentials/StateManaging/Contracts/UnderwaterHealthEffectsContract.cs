using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class UnderwaterHealthEffectsContract
    {

        public float BloodPressureTopBonus;
        public float BloodPressureBottomBonus;
        public float HeartRateBonus;
        public float OxygenLevelBonus;
        public bool LastUnderWaterState;

        public FixedEventContract DrowningDeathEvent;
        public FixedEventContract PlayLightBreath;
        public FixedEventContract PlayMediumBreath;
        public FixedEventContract PlayHardBreath;

    }
}
