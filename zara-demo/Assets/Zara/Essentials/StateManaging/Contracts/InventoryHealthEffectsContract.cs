using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaraEngine.StateManaging
{
    public class InventoryHealthEffectsContract
    {

        public float PlayerWalkSpeedBonus;
        public float PlayerRunSpeedBonus;
        public float PlayerCrouchSpeedBonus;
        public bool IsFreezed;

        public FixedEventContract FreezedByInventoryOverloadEvent;

    }
}
