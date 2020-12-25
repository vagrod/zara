using System;

namespace ZaraEngine.StateManaging
{
    public class HypothermiaMonitorSnippet : SnippetBase
    {

        public HypothermiaMonitorSnippet() : base() { }
        public HypothermiaMonitorSnippet(object contract) : base(contract) { }

        #region Data Fields

        public DateTime? NextCheckTime { get; set; }
        public bool IsDiseaseActivated { get; set; }
        public float CurrentHypothermiaWarmthLevelThreshold { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new HypothermiaMonitorContract
            {
                NextCheckTime = this.NextCheckTime.HasValue ? new DateTimeContract(this.NextCheckTime.Value) : null,
                IsDiseaseActivated = this.IsDiseaseActivated,
                CurrentHypothermiaWarmthLevelThreshold = this.CurrentHypothermiaWarmthLevelThreshold 
            };

            c.HypothermiaDeathEvent = (EventByChanceContract)this.ChildStates["HypothermiaDeathEvent"].ToContract();

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (HypothermiaMonitorContract)o;

            NextCheckTime = (c.NextCheckTime == null || c.NextCheckTime.IsEmpty) ? (DateTime?)null : c.NextCheckTime.ToDateTime();
            IsDiseaseActivated = c.IsDiseaseActivated;
            CurrentHypothermiaWarmthLevelThreshold = c.CurrentHypothermiaWarmthLevelThreshold;

            ChildStates.Clear();

            ChildStates.Add("HypothermiaDeathEvent", new EventByChanceSnippet(c.HypothermiaDeathEvent));
        }

    }
}
