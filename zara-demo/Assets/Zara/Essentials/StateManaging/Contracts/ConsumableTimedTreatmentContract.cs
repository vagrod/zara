using System;

namespace ZaraEngine.StateManaging
{
    public class ConsumableTimedTreatmentContract
    {

        public bool IsNodePart;
        public int TreatedLevel;
        public bool IsFailed;
        public bool IsStarted;
        public DateTime[] ConsumedTimes;
        public int InTimeConsumedCount;
        public bool IsFinished;

    }
}
