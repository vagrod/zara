using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class RunningHealthEffectsContract
    {

        public float BloodPressureTopBonus;
        public float BloodPressureBottomBonus;
        public float HeartRateBonus;
        public float OxygenLevelBonus;
        public float BodyTemperatureBonus;
        public float GameSecondsInRunningState;
        public bool IsWheezeEventTriggered;

        public FixedEventContract IntenseRunningOnEvent;
        public FixedEventContract IntenseRunningOffEvent;

    }
}
