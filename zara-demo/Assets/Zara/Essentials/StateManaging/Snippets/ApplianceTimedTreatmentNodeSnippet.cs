using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class ApplianceTimedTreatmentNodeSnippet : SnippetBase
    {

        public ApplianceTimedTreatmentNodeSnippet() : base() { }
        public ApplianceTimedTreatmentNodeSnippet(object contract) : base(contract) { }

        #region Data Fields

        public List<ApplianceTimedTreatmentSnippet> List { get; set; } = new List<ApplianceTimedTreatmentSnippet>();
        public bool IsOverallHealingStarted { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new ApplianceTimedTreatmentNodeContract
            {
                List = this.List.ToList().ConvertAll(x => (ApplianceTimedTreatmentContract)x.ToContract()).ToArray(),
                IsOverallHealingStarted = this.IsOverallHealingStarted
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ApplianceTimedTreatmentNodeContract)o;

            List = c.List.ToList().ConvertAll(x => new ApplianceTimedTreatmentSnippet(x));
            IsOverallHealingStarted = c.IsOverallHealingStarted;

            ChildStates.Clear();
        }

    }
}
