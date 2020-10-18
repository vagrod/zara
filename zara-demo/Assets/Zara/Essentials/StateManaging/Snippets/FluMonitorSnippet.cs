using System;

namespace ZaraEngine.StateManaging
{
    public class FluMonitorSnippet : SnippetBase
    {

        public FluMonitorSnippet() : base() { }
        public FluMonitorSnippet(object contract) : base(contract) { }

        #region Data Fields

        public DateTime? NextCheckTime { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new FluMonitorContract
            {
                NextCheckTime = this.NextCheckTime.HasValue ? new DateTimeContract(this.NextCheckTime.Value) : null
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (FluMonitorContract)o;

            NextCheckTime = c.NextCheckTime == null || c.NextCheckTime.IsEmpty  ? (DateTime?)null : c.NextCheckTime.ToDateTime();

            ChildStates.Clear();
        }

    }
}
