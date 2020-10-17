using ZaraEngine;

namespace ZaraEngine.StateManaging
{

    public class ZaraEngineState {
        
        public HealthControllerStateContract HealthState;
        public PlayerControllerStateContract BodyState;
        public InventoryControllerStateContract InventoryState;

        public System.DateTime WorldTime;

    }

}