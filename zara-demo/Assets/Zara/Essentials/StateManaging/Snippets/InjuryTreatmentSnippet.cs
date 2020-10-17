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

        

        #endregion 

        public override object ToContract()
        {
            var c = new InjuryTreatmentContract
            {
        
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (InjuryTreatmentContract)o;

        

            ChildStates.Clear();
        }

    }
}
