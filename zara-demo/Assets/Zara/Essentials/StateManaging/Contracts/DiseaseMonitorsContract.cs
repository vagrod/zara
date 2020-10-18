using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class DiseaseMonitorsContract
    {

        public AnginaMonitorContract AnginaMonitor;
        public FluMonitorContract FluMonitor;
        public HyperthermiaMonitorContract HyperthermiaMonitor;
        public HypothermiaMonitorContract HypothermiaMonitor;
        public FoodPoisoningMonitorContract FoodPoisoningMonitor;

    }
}
