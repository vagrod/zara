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

        public ApplianceTimedTreatmentSnippet[] ApplianceTimedTreatments { get; set; } = new ApplianceTimedTreatmentSnippet[] { };
        public ApplianceTimedTreatmentNodeSnippet[] ApplianceTimedTreatmentNodes { get; set; } = new ApplianceTimedTreatmentNodeSnippet[] { };
        public ConsumableTimedTreatmentSnippet[] ConsumableTimedTreatments { get; set; } = new ConsumableTimedTreatmentSnippet[] { };
        public ConsumableTimedTreatmentNodeSnippet[] ConsumableTimedTreatmentNodes { get; set; } = new ConsumableTimedTreatmentNodeSnippet[] { };

        #endregion 

        public override object ToContract()
        {
            var c = new DiseaseTreatmentContract
            {
                ApplianceTimedTreatments = this.ApplianceTimedTreatments.ToList().ConvertAll(x => (ApplianceTimedTreatmentContract)x.ToContract()).ToArray(),
                ApplianceTimedTreatmentNodes = this.ApplianceTimedTreatmentNodes.ToList().ConvertAll(x => (ApplianceTimedTreatmentNodeContract)x.ToContract()).ToArray(),
                ConsumableTimedTreatments = this.ConsumableTimedTreatments.ToList().ConvertAll(x => (ConsumableTimedTreatmentContract)x.ToContract()).ToArray(),
                ConsumableTimedTreatmentNodes = this.ConsumableTimedTreatmentNodes.ToList().ConvertAll(x => (ConsumableTimedTreatmentNodeContract)x.ToContract()).ToArray()
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (DiseaseTreatmentContract)o;

            ApplianceTimedTreatments = c.ApplianceTimedTreatments.ToList().ConvertAll(x => new ApplianceTimedTreatmentSnippet(x)).ToArray();
            ApplianceTimedTreatmentNodes = c.ApplianceTimedTreatmentNodes.ToList().ConvertAll(x => new ApplianceTimedTreatmentNodeSnippet(x)).ToArray();
            ConsumableTimedTreatments = c.ConsumableTimedTreatments.ToList().ConvertAll(x => new ConsumableTimedTreatmentSnippet(x)).ToArray();
            ConsumableTimedTreatmentNodes = c.ConsumableTimedTreatmentNodes.ToList().ConvertAll(x => new ConsumableTimedTreatmentNodeSnippet(x)).ToArray();

            ChildStates.Clear();
        }

    }
}
