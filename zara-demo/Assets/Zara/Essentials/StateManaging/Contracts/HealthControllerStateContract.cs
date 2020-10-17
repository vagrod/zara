using System;
using ZaraEngine;

namespace ZaraEngine.StateManaging
{

    public class HealthControllerStateContract {

        public DateTime LastUpdateGameTime;
        public float PreviousDiseaseVitalsChangeRate;
        public float PreviousInjuryVitalsChangeRate;
        public float HealthCheckCooldownTimer;
        public float VitalsFluctuateEquilibrium;
        public float VilalsFluctuateCheckCounter;
        public bool IsHighPressureEventTriggered;
        public float ActualFatigueValue;
        public bool UnconsciousMode;

        public HealthStateStateContract HealthState;

    }

}