using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class DiseaseTreatmentContract
    {

        public ApplianceTimedTreatmentContract[] ApplianceTimedTreatments;
        public ApplianceTimedTreatmentNodeContract[] ApplianceTimedTreatmentNodes;
        public ConsumableTimedTreatmentContract[] ConsumableTimedTreatments;
        public ConsumableTimedTreatmentNodeContract[] ConsumableTimedTreatmentNodes;

    }
}
