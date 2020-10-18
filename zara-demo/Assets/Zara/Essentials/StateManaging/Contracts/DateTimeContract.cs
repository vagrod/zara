using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class DateTimeContract
    {

        public DateTimeContract() { }
        public DateTimeContract(int day, int month, int year, int hours, int minutes, int seconds, int millisecond)
        {
            Day = day;
            Month = month;
            Year = year;
            Hour = hours;
            Minute = minutes;
            Second = seconds;
            Millisecond = millisecond;
        }
        public DateTimeContract(DateTime date) : this(date.Day, date.Month, date.Year, date.Hour, date.Minute, date.Second, date.Millisecond) { }

        public int Day;
        public int Month;
        public int Year;
        public int Hour;
        public int Minute;
        public int Second;
        public int Millisecond;

        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
        }
    }
}
