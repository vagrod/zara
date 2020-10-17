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

        public ApplianceTimedTreatmentSnippet[] List { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new ApplianceTimedTreatmentNodeContract
            {
                List = this.List.ToList().ConvertAll(x => (ApplianceTimedTreatmentContract)x.ToContract()).ToArray()
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ApplianceTimedTreatmentNodeContract)o;

            List = c.List.ToList().ConvertAll(x => new ApplianceTimedTreatmentSnippet(x)).ToArray();

            ChildStates.Clear();
        }

    }
}
