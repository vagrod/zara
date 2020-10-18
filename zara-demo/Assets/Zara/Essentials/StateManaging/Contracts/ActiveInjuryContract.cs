using System;

namespace ZaraEngine.StateManaging
{
    public class ActiveInjuryContract
    {

        public Guid InjuryId;
        public string InjuryType;

        public bool IsInjuryActivated;
        public bool IsChainInverted;
        public int? TreatedStageLevel;
        public int BodyPart;
        public DateTime InjuryTriggerTime;
        public bool IsTreated;
        public bool IsDiseaseProbabilityChecked;
        public InjuryTreatmentContract Treatments;

    }
}
