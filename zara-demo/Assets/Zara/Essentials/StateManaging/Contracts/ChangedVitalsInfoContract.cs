using System;

namespace ZaraEngine.StateManaging
{
    public class ChangedVitalsInfoContract
    {

        public int Level;
        public float? InitialHeartRate;
        public float? InitialBloodPressureTop;
        public float? InitialBloodPressureBottom;
        public float? InitialBodyTemperature;
        public TimeSpan InitialStageDuration;

    }
}
