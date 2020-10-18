using System;
using ZaraEngine;
using ZaraEngine.StateManaging;

namespace ZaraEngine {

    public static class EngineState {

        public static ZaraEngine.StateManaging.ZaraEngineState GetState(IGameController gc){
            var o = new StateManaging.ZaraEngineState();

            o.WorldTime = new DateTimeContract(gc.WorldTime.Value);

            o.Health = (HealthControllerStateContract)gc.Health.GetState().ToContract();
            o.Inventory = (InventoryControllerStateContract)gc.Inventory.GetState().ToContract();
            //o.Body = (HealthControllerStateContract)gc.Body.GetState().ToContract();

            return o;
        }

        public static void RestoreState(IGameController gc, ZaraEngine.StateManaging.ZaraEngineState state, Action<DateTime> restoreWorldTime){
            restoreWorldTime?.Invoke(state.WorldTime.ToDateTime());

            gc.Health.RestoreState(new HealthControllerStateSnippet(state.Health));
            gc.Inventory.RestoreState(new InventoryControllerStateSnippet(state.Inventory));
        }

    }

}