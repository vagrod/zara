using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class DiseaseTreatmentSnippet : SnippetBase
    {

        public DiseaseTreatmentSnippet() : base() { }
        public DiseaseTreatmentSnippet(object contract) : base(contract) { }

        #region Data Fields

        

        #endregion 

        public override object ToContract()
        {
            var c = new AnginaMonitorContract
            {
        
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (AnginaMonitorContract)o;

        

            ChildStates.Clear();
        }

    }
}
