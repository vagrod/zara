using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class TimeSpanContract
    {

        public TimeSpanContract() { }
        public TimeSpanContract(TimeSpan ts)
        {
            Ticks = ts.Ticks;
        }

        public long Ticks;

        public bool IsEmpty => Ticks == 0;

        public TimeSpan ToTimeSpan()
        {
            return new TimeSpan(Ticks);
        }

    }
}
