using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class InventoryHealthEffectsContract
    {

        public float PlayerWalkSpeedBonus;
        public float PlayerRunSpeedBonus;
        public float PlayerCrouchSpeedBonus;
        public float FatigueDrainBonus;
        public float StaminaDrainBonus;
        public bool IsFreezed;

        public FixedEventContract FreezedByInventoryOverloadEvent;

    }
}
