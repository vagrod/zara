using ZaraEngine;

namespace ZaraEngine.StateManaging
{

    public class ZaraEngineState {
        
        public HealthControllerStateContract Health;
        public PlayerControllerStateContract Body;
        public InventoryControllerStateContract Inventory;

        public System.DateTime WorldTime;

    }

}