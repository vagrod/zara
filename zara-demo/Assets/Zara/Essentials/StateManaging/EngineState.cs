using System;
using ZaraEngine;
using ZaraEngine.StateManaging;

namespace ZaraEngine {

    public static class EngineState {

        public static ZaraEngine.StateManaging.ZaraEngineState GetState(IGameController gc){
            var o = new StateManaging.ZaraEngineState();

            o.WorldTime = gc.WorldTime.Value;

            o.HealthState = (HealthControllerStateContract)gc.Health.GetState().ToContract();
            //o.bodyState = (HealthControllerStateContract)gc.Body.GetState().ToContract();
            //o.inventoryState = (HealthControllerStateContract)gc.Inventory.GetState().ToContract();

            return o;
        }

        public static void RestoreState(IGameController gc, ZaraEngine.StateManaging.ZaraEngineState state, Action<DateTime> restoreWorldTime){
            var hcSnippet = new HealthControllerSnippet();

            restoreWorldTime?.Invoke(state.WorldTime);

            hcSnippet.FromContract(state.HealthState);

            gc.Health.RestoreState(hcSnippet);
        }

    }

}