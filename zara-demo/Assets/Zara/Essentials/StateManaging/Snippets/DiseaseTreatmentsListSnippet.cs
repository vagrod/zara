using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class DiseaseTreatmentsListSnippet : SnippetBase
    {

        public DiseaseTreatmentsListSnippet() : base() { }
        public DiseaseTreatmentsListSnippet(object contract) : base(contract) { }

        #region Data Fields

        public DiseaseTreatmentSnippet[] Data { get; set; } = new DiseaseTreatmentSnippet[] { };

        #endregion 

        public override object ToContract()
        {
            var c = new DiseaseTreatmentsListContract
            {
                Data = this.Data.ToList().ConvertAll(x => (DiseaseTreatmentContract)x.ToContract()).ToArray()
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (DiseaseTreatmentsListContract)o;

            Data = c.Data.ToList().ConvertAll(x => new DiseaseTreatmentSnippet(x)).ToArray();

            ChildStates.Clear();
        }

    }
}
