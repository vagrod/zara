using System;
using System.Collections.Generic;
using ZaraEngine;

namespace ZaraEngine.StateManaging {

    public class HealthControllerSnippet : IStateSnippet
    {

        #region Data Fields

        public DateTime LastUpdateGameTime { get; set; }
        public float PreviousDiseaseVitalsChangeRate { get; set; }
        public float PreviousInjuryVitalsChangeRate { get; set; }
        public float HealthCheckCooldownTimer { get; set; }
        public float VitalsFluctuateEquilibrium { get; set; }
        public float VilalsFluctuateCheckCounter { get; set; }
        public bool IsHighPressureEventTriggered { get; set; }
        public float ActualFatigueValue { get; set; }
        public bool UnconsciousMode { get; set; }

        #endregion 

        public Dictionary<string, IStateSnippet> ChildStates { get; } = new Dictionary<string, IStateSnippet>();

        public object ToContract()
        {
            var c = new HealthControllerStateContract
            {
                LastUpdateGameTime = this.LastUpdateGameTime,
                PreviousDiseaseVitalsChangeRate = this.PreviousDiseaseVitalsChangeRate,
                PreviousInjuryVitalsChangeRate = this.PreviousInjuryVitalsChangeRate,
                HealthCheckCooldownTimer = this.HealthCheckCooldownTimer,
                VitalsFluctuateEquilibrium = this.VitalsFluctuateEquilibrium,
                VilalsFluctuateCheckCounter = this.VilalsFluctuateCheckCounter,
                IsHighPressureEventTriggered = this.IsHighPressureEventTriggered,
                ActualFatigueValue = this.ActualFatigueValue,
                UnconsciousMode = this.UnconsciousMode
            };

            c.HealthState = (HealthStateStateContract)ChildStates["HealthState"].ToContract();

            return c;
        }

        public void FromContract(object o)
        {
            var c = (HealthControllerStateContract)o;

            LastUpdateGameTime = c.LastUpdateGameTime;
            PreviousDiseaseVitalsChangeRate = c.PreviousDiseaseVitalsChangeRate;
            PreviousInjuryVitalsChangeRate = c.PreviousInjuryVitalsChangeRate;
            HealthCheckCooldownTimer = c.HealthCheckCooldownTimer;
            VitalsFluctuateEquilibrium = c.VitalsFluctuateEquilibrium;
            VilalsFluctuateCheckCounter = c.VilalsFluctuateCheckCounter;
            IsHighPressureEventTriggered = c.IsHighPressureEventTriggered;
            ActualFatigueValue = c.ActualFatigueValue;
            UnconsciousMode = c.UnconsciousMode;

            ChildStates.Clear();

            var hc = new HealthStateSnippet();
            hc.FromContract(c.HealthState);

            ChildStates.Add("HealthState", hc);
        }

    }

}