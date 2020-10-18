using System;
using System.Collections.Generic;
using System.Linq;

namespace ZaraEngine.StateManaging
{
    public class ApplianceTimedTreatmentSnippet : SnippetBase
    {

        public ApplianceTimedTreatmentSnippet() : base() { }
        public ApplianceTimedTreatmentSnippet(object contract) : base(contract) { }

        #region Data Fields

        public bool IsNodePart { get; set; }
        public bool IsFailed { get; set; }
        public List<DateTime> ConsumedTimes { get; set; }
        public int InTimeConsumedCount { get; set; }
        public bool IsFinished { get; set; }
        public bool IsStarted { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new ApplianceTimedTreatmentContract
            {
                IsNodePart = this.IsNodePart,
                IsFailed = this.IsFailed,
                ConsumedTimes = this.ConsumedTimes.ConvertAll(x => new DateTimeContract(x)).ToArray(),
                InTimeConsumedCount = this.InTimeConsumedCount,
                IsFinished = this.IsFinished,
                IsStarted = this.IsStarted
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ApplianceTimedTreatmentContract)o;

            IsNodePart = c.IsNodePart;
            IsFailed = c.IsFailed;
            ConsumedTimes = c.ConsumedTimes.ToList().ConvertAll(x => x.ToDateTime());
            InTimeConsumedCount = c.InTimeConsumedCount;
            IsFinished = c.IsFinished;
            IsStarted = c.IsStarted;

            ChildStates.Clear();
        }

    }
}
