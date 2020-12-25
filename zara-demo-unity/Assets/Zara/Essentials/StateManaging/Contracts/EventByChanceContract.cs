using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class EventByChanceContract
    {

        public int ChanceOfHappening;
        public float CoundownTimer;
        public bool IsHappened;
        public bool AutoReset;

    }
}
