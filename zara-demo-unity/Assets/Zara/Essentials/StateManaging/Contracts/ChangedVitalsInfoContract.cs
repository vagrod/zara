using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class ChangedVitalsInfoContract
    {

        public int Level;
        public float? InitialHeartRate;
        public float? InitialBloodPressureTop;
        public float? InitialBloodPressureBottom;
        public float? InitialBodyTemperature;
        public TimeSpanContract InitialStageDuration;

    }
}
