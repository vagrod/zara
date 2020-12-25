using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class ApplianceTimedTreatmentNodeContract
    {

        public ApplianceTimedTreatmentContract[] List;
        public bool IsOverallHealingStarted;

    }
}
