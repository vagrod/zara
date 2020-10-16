using System;
using ZaraEngine;
using ZaraEngine.StateManaging;

namespace ZaraEngine {

    public class EngineState {

        public ZaraEngine.StateManaging.ZaraEngineState GetState(IGameController gc){
            var o = new StateManaging.ZaraEngineState();

            o.healthState = (HealthControllerStateContract)gc.Health.GetState().ToContract();

            return o;
        }

        public void RestoreState(ZaraEngine.StateManaging.ZaraEngineState state){

        }

    }

}