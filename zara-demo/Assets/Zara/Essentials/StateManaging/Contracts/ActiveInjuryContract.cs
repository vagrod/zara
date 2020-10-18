using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class ActiveInjuryContract
    {

        public string InjuryId;
        public string InjuryType;

        public bool IsInjuryActivated;
        public bool IsChainInverted;
        public int? TreatedStageLevel;
        public int BodyPart;
        public DateTimeContract InjuryTriggerTime;
        public bool IsTreated;
        public bool IsDiseaseProbabilityChecked;
        public InjuryTreatmentContract Treatments;

    }
}
