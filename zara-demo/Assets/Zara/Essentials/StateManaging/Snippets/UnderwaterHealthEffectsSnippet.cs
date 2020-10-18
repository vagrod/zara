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

        #endregion 

        public override object ToContract()
        {
            var c = new UnderwaterHealthEffectsContract
            {
                BloodPressureTopBonus = this.BloodPressureTopBonus,
                BloodPressureBottomBonus = this.BloodPressureBottomBonus,
                HeartRateBonus = this.HeartRateBonus,
                OxygenLevelBonus = this.OxygenLevelBonus,
                LastUnderWaterState = this.LastUnderWaterState
            };

            c.DrowningDeathEvent = (FixedEventContract)ChildStates["DrowningDeathEvent"].ToContract();
            c.PlayLightBreath = (FixedEventContract)ChildStates["PlayLightBreath"].ToContract();
            c.PlayMediumBreath = (FixedEventContract)ChildStates["PlayMediumBreath"].ToContract();
            c.PlayHardBreath = (FixedEventContract)ChildStates["PlayHardBreath"].ToContract();

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

            ChildStates.Clear();

            ChildStates.Add("DrowningDeathEvent", new FixedEventSnippet(c.DrowningDeathEvent));
            ChildStates.Add("PlayLightBreath", new FixedEventSnippet(c.PlayLightBreath));
            ChildStates.Add("PlayMediumBreath", new FixedEventSnippet(c.PlayMediumBreath));
            ChildStates.Add("PlayHardBreath", new FixedEventSnippet(c.PlayHardBreath));
        }

    }
}
