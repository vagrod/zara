using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class HyperthermiaMonitorContract
    {

        public DateTimeContract NextCheckTime;
        public bool IsDiseaseActivated;
        public float CurrentHyperthermiaWarmthLevelThreshold;
        public EventByChanceContract HyperthermiaDeathEvent;

    }
}
