using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class ActiveDiseaseContract
    {

        public string DiseaseId;
        public string InjuryId;
        public string DiseaseType;

        public bool IsDiseaseActivated;
        public bool IsSelfHealActive;
        public bool IsChainInverted;
        public DateTimeContract DiseaseStartTime;
        public int? TreatedStageLevel;
        public bool IsTreated;

        public ChangedVitalsInfoContract ChangedVitals;
        public ChangedVitalsInfoContract ChangedCritialStage;
        public DiseaseTreatmentContract Treatments;

    }
}
