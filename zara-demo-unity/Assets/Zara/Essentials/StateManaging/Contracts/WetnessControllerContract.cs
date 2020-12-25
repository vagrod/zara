using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class WetnessControllerContract
    {

        public bool IsWet;
        public DateTimeContract LastGettingWetTime;
        public float WetnessValue;

    }
}
