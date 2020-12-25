using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class ActiveMedicalAgentContract
    {

        public float GameMinutesAgentIsActive;
        public DateTimeContract[] TimesTaken;

    }
}
