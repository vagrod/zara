using System;

namespace ZaraEngine.StateManaging
{
    public class AnginaMonitorSnippet : SnippetBase
    {

        public AnginaMonitorSnippet() : base() { }
        public AnginaMonitorSnippet(object contract) : base(contract) { }

        #region Data Fields

        public DateTime? NextCheckTime { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new AnginaMonitorContract
            {
                NextCheckTime = this.NextCheckTime.HasValue ? new DateTimeContract(this.NextCheckTime.Value) : null
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (AnginaMonitorContract)o;

            NextCheckTime = c.NextCheckTime == null ? (DateTime?)null : c.NextCheckTime.ToDateTime();

            ChildStates.Clear();
        }

    }
}
