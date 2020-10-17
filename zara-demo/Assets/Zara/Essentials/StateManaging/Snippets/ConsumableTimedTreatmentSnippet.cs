using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class ConsumableTimedTreatmentSnippet : SnippetBase
    {

        public ConsumableTimedTreatmentSnippet() : base() { }
        public ConsumableTimedTreatmentSnippet(object contract) : base(contract) { }

        #region Data Fields

        

        #endregion 

        public override object ToContract()
        {
            var c = new ConsumableTimedTreatmentContract
            {
                
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ConsumableTimedTreatmentContract)o;

            

            ChildStates.Clear();
        }

    }
}
