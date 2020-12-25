namespace ZaraEngine.StateManaging
{

    public class EventByChanceSnippet : SnippetBase
    {

        public EventByChanceSnippet() : base() { }
        public EventByChanceSnippet(object contract) : base(contract) { }

        #region Data Fields

        public int ChanceOfHappening { get; set; }
        public float CoundownTimer { get; set; }
        public bool IsHappened { get; set; }
        public bool AutoReset { get; set; }

        #endregion 

        public override object ToContract()
        {
            return new EventByChanceContract
            {
                ChanceOfHappening = this.ChanceOfHappening,
                CoundownTimer = this.CoundownTimer,
                IsHappened = this.IsHappened,
                AutoReset = this.AutoReset
            };
        }

        public override void FromContract(object o)
        {
            var c = (EventByChanceContract)o;

            ChanceOfHappening = c.ChanceOfHappening;
            CoundownTimer = c.CoundownTimer;
            IsHappened = c.IsHappened;
            AutoReset = c.AutoReset;

            ChildStates.Clear();
        }

    }

}