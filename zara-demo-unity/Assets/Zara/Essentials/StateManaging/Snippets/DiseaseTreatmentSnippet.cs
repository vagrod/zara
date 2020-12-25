using System.Collections.Generic;
using System.Linq;

namespace ZaraEngine.StateManaging
{
    public class DiseaseTreatmentSnippet : SnippetBase
    {

        public DiseaseTreatmentSnippet() : base() { }
        public DiseaseTreatmentSnippet(object contract) : base(contract) { }

        #region Data Fields

        public List<ApplianceTimedTreatmentSnippet> ApplianceTimedTreatments { get; set; } = new List<ApplianceTimedTreatmentSnippet>() { };
        public List<ApplianceTimedTreatmentNodeSnippet> ApplianceTimedTreatmentNodes { get; set; } = new List<ApplianceTimedTreatmentNodeSnippet>() { };
        public List<ConsumableTimedTreatmentSnippet> ConsumableTimedTreatments { get; set; } = new List<ConsumableTimedTreatmentSnippet>() { };
        public List<ConsumableTimedTreatmentNodeSnippet> ConsumableTimedTreatmentNodes { get; set; } = new List<ConsumableTimedTreatmentNodeSnippet>() { };

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

            ApplianceTimedTreatments = c.ApplianceTimedTreatments.ToList().ConvertAll(x => new ApplianceTimedTreatmentSnippet(x));
            ApplianceTimedTreatmentNodes = c.ApplianceTimedTreatmentNodes.ToList().ConvertAll(x => new ApplianceTimedTreatmentNodeSnippet(x));
            ConsumableTimedTreatments = c.ConsumableTimedTreatments.ToList().ConvertAll(x => new ConsumableTimedTreatmentSnippet(x));
            ConsumableTimedTreatmentNodes = c.ConsumableTimedTreatmentNodes.ToList().ConvertAll(x => new ConsumableTimedTreatmentNodeSnippet(x));

            ChildStates.Clear();
        }

    }
}
