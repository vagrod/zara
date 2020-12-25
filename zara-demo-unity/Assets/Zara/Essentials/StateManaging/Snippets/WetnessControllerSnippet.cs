using System;

namespace ZaraEngine.StateManaging
{
    public class WetnessControllerSnippet : SnippetBase
    {

        public WetnessControllerSnippet() : base() { }
        public WetnessControllerSnippet(object contract) : base(contract) { }

        #region Data Fields

        public bool IsWet { get; set; }
        public DateTime? LastGettingWetTime { get; set; }
        public float WetnessValue { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new WetnessControllerContract
            {
                IsWet = this.IsWet,
                LastGettingWetTime = this.LastGettingWetTime.HasValue ? new DateTimeContract(LastGettingWetTime.Value) : null,
                WetnessValue = this.WetnessValue
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (WetnessControllerContract)o;

            IsWet = c.IsWet;
            LastGettingWetTime = c.LastGettingWetTime == null || c.LastGettingWetTime.IsEmpty ? (DateTime?)null : c.LastGettingWetTime.ToDateTime();
            WetnessValue = c.WetnessValue;

            ChildStates.Clear();
        }

    }
}
