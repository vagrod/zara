using System;
using System.Collections.Generic;
using System.Linq;
using ZaraEngine.Diseases;

namespace ZaraEngine.StateManaging
{
    public class ConsumableTimedTreatmentSnippet : SnippetBase
    {

        public ConsumableTimedTreatmentSnippet() : base() { }
        public ConsumableTimedTreatmentSnippet(object contract) : base(contract) { }

        #region Data Fields

        public bool IsNodePart { get; set; }
        public DiseaseLevels TreatedLevel { get; set; }
        public bool IsFailed { get; set; }
        public bool IsStarted { get; set; }
        public List<DateTime> ConsumedTimes { get; set; }
        public int InTimeConsumedCount { get; set; }
        public bool IsFinished { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new ConsumableTimedTreatmentContract
            {
                IsNodePart = this.IsNodePart,
                TreatedLevel = (int)this.TreatedLevel,
                IsFailed = this.IsFailed,
                IsStarted = this.IsStarted,
                ConsumedTimes = this.ConsumedTimes.ConvertAll(x => new DateTimeContract(x)).ToArray(),
                InTimeConsumedCount = this.InTimeConsumedCount,
                IsFinished = this.IsFinished
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ConsumableTimedTreatmentContract)o;

            IsNodePart = c.IsNodePart;
            TreatedLevel = (DiseaseLevels)c.TreatedLevel;
            IsFailed = c.IsFailed;
            IsStarted = c.IsStarted;
            ConsumedTimes = c.ConsumedTimes.ToList().ConvertAll(x => x.ToDateTime());
            InTimeConsumedCount = c.InTimeConsumedCount;
            IsFinished = c.IsFinished;

            ChildStates.Clear();
        }

    }
}
