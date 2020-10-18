using System;

namespace ZaraEngine.StateManaging
{
    [Serializable]
    public class InventoryHealthEffectsContract
    {

        public float PlayerWalkSpeedBonus;
        public float PlayerRunSpeedBonus;
        public float PlayerCrouchSpeedBonus;
        public bool IsFreezed;

        public FixedEventContract FreezedByInventoryOverloadEvent;

    }
}
