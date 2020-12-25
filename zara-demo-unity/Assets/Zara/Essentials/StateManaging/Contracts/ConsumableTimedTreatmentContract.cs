using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class ConsumableTimedTreatmentContract
    {

        public bool IsNodePart;
        public int TreatedLevel;
        public bool IsFailed;
        public bool IsStarted;
        public DateTimeContract[] ConsumedTimes;
        public int InTimeConsumedCount;
        public bool IsFinished;

    }
}
