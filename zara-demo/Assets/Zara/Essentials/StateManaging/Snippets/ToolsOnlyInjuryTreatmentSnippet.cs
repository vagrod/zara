using System;

namespace ZaraEngine.StateManaging
{
    public class ToolsOnlyInjuryTreatmentSnippet : SnippetBase
    {

        public ToolsOnlyInjuryTreatmentSnippet() : base() { }
        public ToolsOnlyInjuryTreatmentSnippet(object contract) : base(contract) { }

        #region Data Fields

        public DateTime? LastToolTime { get; set; }
        public int ToolsUsed { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new ToolsOnlyInjuryTreatmentContract
            {
                LastToolTime = this.LastToolTime.HasValue ? new DateTimeContract(this.LastToolTime.Value) : null,
                ToolsUsed = this.ToolsUsed
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ToolsOnlyInjuryTreatmentContract)o;

            LastToolTime = c.LastToolTime == null || c.LastToolTime.IsEmpty ? (DateTime?)null : c.LastToolTime.ToDateTime();
            ToolsUsed = c.ToolsUsed;

            ChildStates.Clear();
        }

    }
}
