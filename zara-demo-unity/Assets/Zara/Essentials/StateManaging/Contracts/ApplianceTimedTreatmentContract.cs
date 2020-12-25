using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class ApplianceTimedTreatmentContract
    {

        public bool IsNodePart;
        public bool IsFailed;
        public DateTimeContract[] ConsumedTimes;
        public int InTimeConsumedCount;
        public bool IsFinished;
        public bool IsStarted;

    }
}
