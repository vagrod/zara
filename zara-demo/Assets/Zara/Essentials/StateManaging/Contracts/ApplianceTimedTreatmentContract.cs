using System;

namespace ZaraEngine.StateManaging
{
    public class ApplianceTimedTreatmentContract
    {

        public bool IsNodePart;
        public bool IsFailed;
        public DateTime[] ConsumedTimes;
        public int InTimeConsumedCount;
        public bool IsFinished;
        public bool IsStarted;

    }
}
