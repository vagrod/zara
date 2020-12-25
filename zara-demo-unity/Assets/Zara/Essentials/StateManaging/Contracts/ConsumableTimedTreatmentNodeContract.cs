using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class ConsumableTimedTreatmentNodeContract
    {

        public ConsumableTimedTreatmentContract[] List;
        public bool IsOverallHealingStarted;

    }
}
