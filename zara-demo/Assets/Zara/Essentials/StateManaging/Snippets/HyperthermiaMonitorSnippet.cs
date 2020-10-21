using System;

namespace ZaraEngine.StateManaging
{
    public class HyperthermiaMonitorSnippet : SnippetBase
    {

        public HyperthermiaMonitorSnippet() : base() { }
        public HyperthermiaMonitorSnippet(object contract) : base(contract) { }

        #region Data Fields

        public DateTime? NextCheckTime { get; set; }
        public bool IsDiseaseActivated { get; set; }
        public float CurrentHyperthermiaWarmthLevelThreshold { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new HyperthermiaMonitorContract
            {
                NextCheckTime = this.NextCheckTime.HasValue ? new DateTimeContract(this.NextCheckTime.Value) : null,
                IsDiseaseActivated = this.IsDiseaseActivated,
                CurrentHyperthermiaWarmthLevelThreshold = this.CurrentHyperthermiaWarmthLevelThreshold 
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (HyperthermiaMonitorContract)o;

            NextCheckTime = (c.NextCheckTime == null || c.NextCheckTime.IsEmpty) ? (DateTime?)null : c.NextCheckTime.ToDateTime();
            IsDiseaseActivated = c.IsDiseaseActivated;
            CurrentHyperthermiaWarmthLevelThreshold = c.CurrentHyperthermiaWarmthLevelThreshold;

            ChildStates.Clear();
        }

    }
}
