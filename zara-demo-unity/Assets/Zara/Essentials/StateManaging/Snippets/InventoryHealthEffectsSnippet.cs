namespace ZaraEngine.StateManaging
{
    public class InventoryHealthEffectsSnippet : SnippetBase
    {

        public InventoryHealthEffectsSnippet() : base() { }
        public InventoryHealthEffectsSnippet(object contract) : base(contract) { }

        #region Data Fields

        public float PlayerWalkSpeedBonus { get; set; }
        public float PlayerRunSpeedBonus { get; set; }
        public float PlayerCrouchSpeedBonus { get; set; }
        public bool IsFreezed { get; set; }

        #endregion 

        public override object ToContract()
        {
            var c = new InventoryHealthEffectsContract
            {
                PlayerWalkSpeedBonus = this.PlayerWalkSpeedBonus,
                PlayerRunSpeedBonus = this.PlayerRunSpeedBonus,
                PlayerCrouchSpeedBonus = this.PlayerCrouchSpeedBonus,
                IsFreezed = this.IsFreezed
            };

            c.FreezedByInventoryOverloadEvent = (FixedEventContract)ChildStates["FreezedByInventoryOverloadEvent"].ToContract();

            return c;
        }

        public override void FromContract(object o)
        {
            var c = (InventoryHealthEffectsContract)o;

            PlayerWalkSpeedBonus = c.PlayerWalkSpeedBonus;
            PlayerRunSpeedBonus = c.PlayerRunSpeedBonus;
            PlayerCrouchSpeedBonus = c.PlayerCrouchSpeedBonus;
            IsFreezed = c.IsFreezed;

            ChildStates.Clear();

            ChildStates.Add("FreezedByInventoryOverloadEvent", new FixedEventSnippet(c.FreezedByInventoryOverloadEvent));
        }

    }
}
