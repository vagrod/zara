using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class InjuryTreatmentsListSnippet : SnippetBase
    {

        public InjuryTreatmentsListSnippet() : base() { }
        public InjuryTreatmentsListSnippet(object contract) : base(contract) { }

        #region Data Fields

        public InjuryTreatmentSnippet[] Data { get; set; } = new InjuryTreatmentSnippet[] { };

        #endregion 

        public override object ToContract()
        {
            var c = new InjuryTreatmentsListContract
            {
                Data = this.Data.ToList().ConvertAll(x => (InjuryTreatmentContract)x.ToContract()).ToArray()
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (InjuryTreatmentsListContract)o;

            Data = c.Data.ToList().ConvertAll(x => new InjuryTreatmentSnippet(x)).ToArray();

            ChildStates.Clear();
        }

    }
}
