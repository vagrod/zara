using System;

namespace ZaraEngine.StateManaging
{
    public class UnderwaterHealthEffectsSnippet : SnippetBase
    {

        public UnderwaterHealthEffectsSnippet() : base() { }
        public UnderwaterHealthEffectsSnippet(object contract) : base(contract) { }

        #region Data Fields

        public float BloodPressureTopBonus { get; set; }
        public float BloodPressureBottomBonus { get; set; }
        public float HeartRateBonus { get; set; }
        public float OxygenLevelBonus { get; set; }
        public bool LastUnderWaterState { get; set; }
        public DateTime? GameTimeGotUnderwater { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new UnderwaterHealthEffectsContract
            {
                BloodPressureTopBonus = this.BloodPressureTopBonus,
                BloodPressureBottomBonus = this.BloodPressureBottomBonus,
                HeartRateBonus = this.HeartRateBonus,
                OxygenLevelBonus = this.OxygenLevelBonus,
                LastUnderWaterState = this.LastUnderWaterState,
                GameTimeGotUnderwater = this.GameTimeGotUnderwater.HasValue ? new DateTimeContract(this.GameTimeGotUnderwater.Value) : null
            };

            c.DrowningDeathEvent = (FixedEventContract)ChildStates["DrowningDeathEvent"].ToContract();

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (UnderwaterHealthEffectsContract)o;

            BloodPressureTopBonus = c.BloodPressureTopBonus;
            BloodPressureBottomBonus = c.BloodPressureBottomBonus;
            HeartRateBonus = c.HeartRateBonus;
            OxygenLevelBonus = c.OxygenLevelBonus;
            LastUnderWaterState = c.LastUnderWaterState;
            GameTimeGotUnderwater = c.GameTimeGotUnderwater == null || c.GameTimeGotUnderwater.IsEmpty ? (DateTime?)null : c.GameTimeGotUnderwater.ToDateTime();

            ChildStates.Clear();

            ChildStates.Add("DrowningDeathEvent", new FixedEventSnippet(c.DrowningDeathEvent));
        }

    }
}
