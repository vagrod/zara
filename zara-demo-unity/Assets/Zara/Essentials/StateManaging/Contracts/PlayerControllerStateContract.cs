using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class PlayerControllerStateContract {

        public string[] Clothes;
        public MedicalBodyApplianceContract[] Appliances;
        public WetnessControllerContract WetnessController;

        public float WarmthLevelTimeoutCounter;
        public float WetnessLevelTimeoutCounter;
        public float WarmthLerpTarget;
        public float? WarmthLerpCounter;
        public float WarmthLerpBase;
        public float SleepingCounter;
        public float SleepDurationGameHours;
        public float SleepHealthCheckPeriod;
        public int SleepHealthChecksLeft;
        public DateTimeContract SleepStartTime;
        public float FatigueValueAfterSleep;

    }

}