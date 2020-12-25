namespace ZaraEngine.StateManaging
{
    public class FatigueHealthEffectsSnippet : SnippetBase
    {

        public FatigueHealthEffectsSnippet() : base() { }
        public FatigueHealthEffectsSnippet(object contract) : base(contract) { }

        #region Data Fields

        public float BloodPressureTopBonus { get; set; }
        public float BloodPressureBottomBonus { get; set; }
        public float HeartRateBonus { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new FatigueHealthEffectsContract
            {
                BloodPressureTopBonus = this.BloodPressureTopBonus,
                BloodPressureBottomBonus = this.BloodPressureBottomBonus,
                HeartRateBonus = this.HeartRateBonus
            };

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (FatigueHealthEffectsContract)o;

            BloodPressureTopBonus = c.BloodPressureTopBonus;
            BloodPressureBottomBonus = c.BloodPressureBottomBonus;
            HeartRateBonus = c.HeartRateBonus;

            ChildStates.Clear();
        }

    }
}
