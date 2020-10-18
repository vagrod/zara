using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class InjuryTreatmentSnippet : SnippetBase
    {

        public InjuryTreatmentSnippet() : base() { }
        public InjuryTreatmentSnippet(object contract) : base(contract) { }

        #region Data Fields

        public List<ToolsOnlyInjuryTreatmentSnippet> ToolsOnlyTreatments { get; set; } = new List<ToolsOnlyInjuryTreatmentSnippet>() { };

        #endregion 

        public override object ToContract()
        {
            var c = new InjuryTreatmentContract
            {
                ToolsOnlyTreatments = this.ToolsOnlyTreatments.ToList().ConvertAll(x => (ToolsOnlyInjuryTreatmentContract)x.ToContract()).ToArray()
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (InjuryTreatmentContract)o;

            ToolsOnlyTreatments = c.ToolsOnlyTreatments.ToList().ConvertAll(x => new ToolsOnlyInjuryTreatmentSnippet(x));

            ChildStates.Clear();
        }

    }
}
