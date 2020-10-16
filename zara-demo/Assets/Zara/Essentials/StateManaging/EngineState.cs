using System;
using ZaraEngine;
using ZaraEngine.StateManaging;

namespace ZaraEngine {

    public static class EngineState {

        public static ZaraEngine.StateManaging.ZaraEngineState GetState(IGameController gc){
            var o = new StateManaging.ZaraEngineState();

            o.healthState = (HealthControllerStateContract)gc.Health.GetState().ToContract();
            //o.bodyState = (HealthControllerStateContract)gc.Body.GetState().ToContract();
            //o.inventoryState = (HealthControllerStateContract)gc.Inventory.GetState().ToContract();

            return o;
        }

        public static void RestoreState(IGameController gc, ZaraEngine.StateManaging.ZaraEngineState state){
            var hcSnippet = new HealthControllerSnippet();

            hcSnippet.FromContract(state.healthState);

            gc.Health.RestoreState(hcSnippet);
        }

    }

}