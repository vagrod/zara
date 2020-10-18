namespace ZaraEngine.StateManaging
{
    public class RunningHealthEffectsSnippet : SnippetBase
    {

        public RunningHealthEffectsSnippet() : base() { }
        public RunningHealthEffectsSnippet(object contract) : base(contract) { }

        #region Data Fields

        public float BloodPressureTopBonus { get; set; }
        public float BloodPressureBottomBonus { get; set; }
        public float HeartRateBonus { get; set; }
        public float OxygenLevelBonus { get; set; }
        public float BodyTemperatureBonus { get; set; }
        public float GameSecondsInRunningState { get; set; }
        public bool IsWheezeEventTriggered { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new RunningHealthEffectsContract
            {
                BloodPressureTopBonus = this.BloodPressureTopBonus,
                BloodPressureBottomBonus = this.BloodPressureBottomBonus,
                HeartRateBonus = this.HeartRateBonus,
                OxygenLevelBonus = this.OxygenLevelBonus,
                BodyTemperatureBonus = this.BodyTemperatureBonus,
                GameSecondsInRunningState = this.GameSecondsInRunningState,
                IsWheezeEventTriggered = this.IsWheezeEventTriggered
            };

            c.IntenseRunningOnEvent = (FixedEventContract)ChildStates["IntenseRunningOnEvent"].ToContract();
            c.IntenseRunningOffEvent = (FixedEventContract)ChildStates["IntenseRunningOffEvent"].ToContract();

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (RunningHealthEffectsContract)o;

            BloodPressureTopBonus = c.BloodPressureTopBonus;
            BloodPressureBottomBonus = c.BloodPressureBottomBonus;
            HeartRateBonus = c.HeartRateBonus;
            OxygenLevelBonus = c.OxygenLevelBonus;
            BodyTemperatureBonus = c.BodyTemperatureBonus;
            GameSecondsInRunningState = c.GameSecondsInRunningState;
            IsWheezeEventTriggered = c.IsWheezeEventTriggered;

            ChildStates.Clear();

            ChildStates.Add("IntenseRunningOnEvent", new FixedEventSnippet(c.IntenseRunningOnEvent));
            ChildStates.Add("IntenseRunningOffEvent", new FixedEventSnippet(c.IntenseRunningOffEvent));
        }

    }
}
