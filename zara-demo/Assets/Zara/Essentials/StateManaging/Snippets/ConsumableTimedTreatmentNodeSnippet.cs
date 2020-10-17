using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class ConsumableTimedTreatmentNodeSnippet : SnippetBase
    {

        public ConsumableTimedTreatmentNodeSnippet() : base() { }
        public ConsumableTimedTreatmentNodeSnippet(object contract) : base(contract) { }

        #region Data Fields

        public ConsumableTimedTreatmentSnippet[] List { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new ConsumableTimedTreatmentNodeContract
            {
                List = this.List.ToList().ConvertAll(x => (ConsumableTimedTreatmentContract)x.ToContract()).ToArray()
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ConsumableTimedTreatmentNodeContract)o;

            List = c.List.ToList().ConvertAll(x => new ConsumableTimedTreatmentSnippet(x)).ToArray();

            ChildStates.Clear();
        }

    }
}
