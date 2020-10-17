using System;
using ZaraEngine;
using ZaraEngine.StateManaging;

namespace ZaraEngine {

    public static class EngineState {

        public static ZaraEngine.StateManaging.ZaraEngineState GetState(IGameController gc){
            var o = new StateManaging.ZaraEngineState();

            o.WorldTime = gc.WorldTime.Value;

            o.Health = (HealthControllerStateContract)gc.Health.GetState().ToContract();
            //o.Body = (HealthControllerStateContract)gc.Body.GetState().ToContract();
            //o.Inventory = (HealthControllerStateContract)gc.Inventory.GetState().ToContract();

            return o;
        }

        public static void RestoreState(IGameController gc, ZaraEngine.StateManaging.ZaraEngineState state, Action<DateTime> restoreWorldTime){
            var hcSnippet = new HealthControllerSnippet();

            restoreWorldTime?.Invoke(state.WorldTime);

            hcSnippet.FromContract(state.Health);

            gc.Health.RestoreState(hcSnippet);
        }

    }

}