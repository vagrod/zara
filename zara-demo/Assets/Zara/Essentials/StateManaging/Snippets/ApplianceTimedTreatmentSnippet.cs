using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class ApplianceTimedTreatmentSnippet : SnippetBase
    {

        public ApplianceTimedTreatmentSnippet() : base() { }
        public ApplianceTimedTreatmentSnippet(object contract) : base(contract) { }

        #region Data Fields

        

        #endregion 

        public override object ToContract()
        {
            var c = new ApplianceTimedTreatmentContract
            {
                
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ApplianceTimedTreatmentContract)o;

            

            ChildStates.Clear();
        }

    }
}
