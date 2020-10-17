using System;

namespace ZaraEngine.StateManaging
{
    public class ActiveDiseaseContract
    {

        public Guid DiseaseId;
        public Guid? InjuryId;
        public string DiseaseType;

        public bool IsDiseaseActivated;
        public bool IsSelfHealActive;
        public bool IsChainInverted;
        public DateTime DiseaseStartTime;
        public int? TreatedStageLevel;
        public bool IsTreated;

        public ChangedVitalsInfoContract ChangedVitals;
        public ChangedVitalsInfoContract ChangedCritialStage;
        public DiseaseTreatmentsListContract Treatments;

    }
}
