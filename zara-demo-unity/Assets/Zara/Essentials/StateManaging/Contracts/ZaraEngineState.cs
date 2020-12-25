using System;

namespace ZaraEngine.StateManaging
{

    [Serializable]
    public class ZaraEngineState {
        
        public HealthControllerStateContract Health;
        public PlayerControllerStateContract Body;
        public InventoryControllerStateContract Inventory;

        public DateTimeContract WorldTime;

    }

}