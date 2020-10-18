using System;

namespace ZaraEngine.StateManaging
{
    public class TimeSpanContract
    {

        public TimeSpanContract() { }
        public TimeSpanContract(TimeSpan ts)
        {
            Ticks = ts.Ticks;
        }

        public long Ticks;

        public TimeSpan ToTimeSpan()
        {
            return new TimeSpan(Ticks);
        }

    }
}
