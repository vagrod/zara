using System;
using ZaraEngine;
using ZaraEngine.StateManaging;

namespace ZaraEngine {

    public static class EngineState {

        public static ZaraEngine.StateManaging.ZaraEngineState GetState(IGameController gc){
            var o = new StateManaging.ZaraEngineState();

            o.healthState = (HealthControllerStateContract)gc.Health.GetState().ToContract();

            return o;
        }

        public static void RestoreState(ZaraEngine.StateManaging.ZaraEngineState state){

        }

    }

}