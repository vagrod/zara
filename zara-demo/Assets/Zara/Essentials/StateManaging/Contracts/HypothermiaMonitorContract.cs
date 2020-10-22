using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class HypothermiaMonitorContract
    {

        public DateTimeContract NextCheckTime;
        public bool IsDiseaseActivated;
        public float CurrentHypothermiaWarmthLevelThreshold;
        public EventByChanceContract HypothermiaDeathEvent;

    }
}
