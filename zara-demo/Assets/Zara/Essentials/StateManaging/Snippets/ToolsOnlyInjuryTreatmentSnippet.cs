using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                LastToolTime = this.LastToolTime,
                ToolsUsed = this.ToolsUsed
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ToolsOnlyInjuryTreatmentContract)o;

            LastToolTime = c.LastToolTime;
            ToolsUsed = c.ToolsUsed;

            ChildStates.Clear();
        }

    }
}
