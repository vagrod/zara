using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases;

namespace ZaraEngine.StateManaging
{
    public class ActiveDiseaseSnippet : SnippetBase
    {

        public ActiveDiseaseSnippet() : base() { }
        public ActiveDiseaseSnippet(object contract) : base(contract) { }

        #region Data Fields

        public Guid DiseaseId { get; set; }
        public Guid? InjuryId { get; set; }
        public string DiseaseType { get; set; }

        public DiseaseTreatmentSnippet Treatments { get; set; }

        public bool IsDiseaseActivated { get; set; }
        public bool IsSelfHealActive { get; set; }
        public bool IsChainInverted { get; set; }
        public DateTime DiseaseStartTime { get; set; }
        public DiseaseLevels? TreatedStageLevel { get; set; }
        public bool IsTreated { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new ActiveDiseaseContract
            {
                DiseaseId = this.DiseaseId,
                InjuryId = this.InjuryId,
                IsDiseaseActivated = this.IsDiseaseActivated,
                IsSelfHealActive = this.IsSelfHealActive,
                IsChainInverted = this.IsChainInverted,
                DiseaseStartTime = this.DiseaseStartTime,
                TreatedStageLevel = this.TreatedStageLevel.HasValue ? (int?)this.TreatedStageLevel.Value : (int?)null,
                IsTreated = this.IsTreated,
                DiseaseType = this.DiseaseType
            };

            c.ChangedVitals = ChildStates["ChangedVitals"] == null ? null :(ChangedVitalsInfoContract)ChildStates["ChangedVitals"].ToContract();
            c.ChangedCritialStage = ChildStates["ChangedCritialStage"] == null ? null : (ChangedVitalsInfoContract)ChildStates["ChangedCritialStage"].ToContract();
            c.Treatments = (DiseaseTreatmentContract)ChildStates["Treatments"].ToContract();

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (ActiveDiseaseContract)o;

            DiseaseId = c.DiseaseId;
            InjuryId = c.InjuryId;
            IsDiseaseActivated = c.IsDiseaseActivated;
            IsSelfHealActive = c.IsSelfHealActive;
            IsChainInverted = c.IsChainInverted;
            DiseaseStartTime = c.DiseaseStartTime;
            TreatedStageLevel = c.TreatedStageLevel.HasValue ? (DiseaseLevels?)c.TreatedStageLevel.Value : (DiseaseLevels?)null;
            IsTreated = c.IsTreated;
            DiseaseType = c.DiseaseType;

            ChildStates.Clear();

            ChildStates.Add("ChangedVitals", c.ChangedVitals == null ?  null : new ChangedVitalsInfoSnippet(c.ChangedVitals));
            ChildStates.Add("ChangedCritialStage", c.ChangedCritialStage == null ? null :  new ChangedVitalsInfoSnippet(c.ChangedCritialStage));
            ChildStates.Add("Treatments", new DiseaseTreatmentSnippet(c.Treatments));
        }

    }
}
