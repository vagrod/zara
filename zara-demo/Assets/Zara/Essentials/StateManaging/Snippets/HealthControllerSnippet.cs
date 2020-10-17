using System;
using System.Collections.Generic;
using ZaraEngine;

namespace ZaraEngine.StateManaging {

    public class HealthControllerSnippet : SnippetBase
    {

        public HealthControllerSnippet() : base() { }
        public HealthControllerSnippet(object contract) : base(contract) { }

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

        public override object ToContract()
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

            c.DiseaseDizzinessEvent = (EventByChanceContract)ChildStates["DiseaseDizzinessEvent"].ToContract();
            c.DiseaseBlackoutsEvent = (EventByChanceContract)ChildStates["DiseaseBlackoutsEvent"].ToContract();
            c.DiseaseDeathEvent = (EventByChanceContract)ChildStates["DiseaseDeathEvent"].ToContract();
            c.SneezeEvent = (EventByChanceContract)ChildStates["SneezeEvent"].ToContract();
            c.CoughEvent = (EventByChanceContract)ChildStates["CoughEvent"].ToContract();
            c.BloodLevelDizzinessEvent = (EventByChanceContract)ChildStates["BloodLevelDizzinessEvent"].ToContract();
            c.BloodLevelBlackoutsEvent = (EventByChanceContract)ChildStates["BloodLevelBlackoutsEvent"].ToContract();
            c.LowBodyTemperatureDizzinessEvent = (EventByChanceContract)ChildStates["LowBodyTemperatureDizzinessEvent"].ToContract();
            c.LowBodyTemperatureBlackoutsEvent = (EventByChanceContract)ChildStates["LowBodyTemperatureBlackoutsEvent"].ToContract();
            c.BloodLevelDeathEvent = (EventByChanceContract)ChildStates["BloodLevelDeathEvent"].ToContract();
            c.DehydrationDeathEvent = (EventByChanceContract)ChildStates["DehydrationDeathEvent"].ToContract();
            c.StarvationDeathEvent = (EventByChanceContract)ChildStates["StarvationDeathEvent"].ToContract();
            c.VitalsDeathEvent = (EventByChanceContract)ChildStates["VitalsDeathEvent"].ToContract();
            c.OverdoseDeathEvent = (EventByChanceContract)ChildStates["OverdoseDeathEvent"].ToContract();
            c.HeartFailureDeathEvent = (EventByChanceContract)ChildStates["HeartFailureDeathEvent"].ToContract();
            c.LsdEffect = (EventByChanceContract)ChildStates["LsdEffect"].ToContract();
            c.FatigueDizzinessEvent = (EventByChanceContract)ChildStates["FatigueDizzinessEvent"].ToContract();
            c.FatigueBlackoutsEvent = (EventByChanceContract)ChildStates["FatigueBlackoutsEvent"].ToContract();
            c.FatigueSleepEvent = (EventByChanceContract)ChildStates["FatigueSleepEvent"].ToContract();
            c.SedativeSleepEvent = (EventByChanceContract)ChildStates["SedativeSleepEvent"].ToContract();

            c.HighPressureEvent = (FixedEventContract)ChildStates["HighPressureEvent"].ToContract();
            c.NormalPressureEvent = (FixedEventContract)ChildStates["NormalPressureEvent"].ToContract();
            c.DrowningEvent = (FixedEventContract)ChildStates["DrowningEvent"].ToContract();

            c.UnderwaterHealthEffects = (UnderwaterHealthEffectsContract)ChildStates["UnderwaterHealthEffects"].ToContract();
            c.RunningHealthEffects = (RunningHealthEffectsContract)ChildStates["RunningHealthEffects"].ToContract();
            c.FatigueHealthEffects = (FatigueHealthEffectsContract)ChildStates["FatigueHealthEffects"].ToContract();
            c.InventoryHealthEffects = (InventoryHealthEffectsContract)ChildStates["InventoryHealthEffects"].ToContract();
            c.MedicalAgentsHealthEffects = (MedicalAgentsHealthEffectsContract)ChildStates["MedicalAgentsHealthEffects"].ToContract();
            c.ConsumablesHealthEffects = (ConsumablesHealthEffectsContract)ChildStates["ConsumablesHealthEffects"].ToContract();
            c.ClothesHealthEffects = (ClothesHealthEffectsContract)ChildStates["ClothesHealthEffects"].ToContract();

            return c;
        }

        public override void FromContract(object o)
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

            ChildStates.Add("HealthState", new HealthStateSnippet(c.HealthState));

            ChildStates.Add("DiseaseDizzinessEvent", new EventByChanceSnippet(c.DiseaseDizzinessEvent));
            ChildStates.Add("DiseaseBlackoutsEvent", new EventByChanceSnippet(c.DiseaseBlackoutsEvent));
            ChildStates.Add("DiseaseDeathEvent", new EventByChanceSnippet(c.DiseaseDeathEvent));
            ChildStates.Add("SneezeEvent", new EventByChanceSnippet(c.SneezeEvent));
            ChildStates.Add("CoughEvent", new EventByChanceSnippet(c.CoughEvent));
            ChildStates.Add("BloodLevelDizzinessEvent", new EventByChanceSnippet(c.BloodLevelDizzinessEvent));
            ChildStates.Add("BloodLevelBlackoutsEvent", new EventByChanceSnippet(c.BloodLevelBlackoutsEvent));
            ChildStates.Add("LowBodyTemperatureDizzinessEvent", new EventByChanceSnippet(c.LowBodyTemperatureDizzinessEvent));
            ChildStates.Add("LowBodyTemperatureBlackoutsEvent", new EventByChanceSnippet(c.LowBodyTemperatureBlackoutsEvent));
            ChildStates.Add("BloodLevelDeathEvent", new EventByChanceSnippet(c.BloodLevelDeathEvent));
            ChildStates.Add("DehydrationDeathEvent", new EventByChanceSnippet(c.DehydrationDeathEvent));
            ChildStates.Add("StarvationDeathEvent", new EventByChanceSnippet(c.StarvationDeathEvent));
            ChildStates.Add("VitalsDeathEvent", new EventByChanceSnippet(c.VitalsDeathEvent));
            ChildStates.Add("OverdoseDeathEvent", new EventByChanceSnippet(c.OverdoseDeathEvent));
            ChildStates.Add("HeartFailureDeathEvent", new EventByChanceSnippet(c.HeartFailureDeathEvent));
            ChildStates.Add("LsdEffect", new EventByChanceSnippet(c.LsdEffect));
            ChildStates.Add("FatigueDizzinessEvent", new EventByChanceSnippet(c.FatigueDizzinessEvent));
            ChildStates.Add("FatigueBlackoutsEvent", new EventByChanceSnippet(c.FatigueBlackoutsEvent));
            ChildStates.Add("FatigueSleepEvent", new EventByChanceSnippet(c.FatigueSleepEvent));
            ChildStates.Add("SedativeSleepEvent", new EventByChanceSnippet(c.SedativeSleepEvent));

            ChildStates.Add("HighPressureEvent", new FixedEventSnippet(c.HighPressureEvent));
            ChildStates.Add("NormalPressureEvent", new FixedEventSnippet(c.NormalPressureEvent));
            ChildStates.Add("DrowningEvent", new FixedEventSnippet(c.DrowningEvent));

            ChildStates.Add("UnderwaterHealthEffects", new UnderwaterHealthEffectsSnippet(c.UnderwaterHealthEffects));
            ChildStates.Add("RunningHealthEffects", new RunningHealthEffectsSnippet(c.RunningHealthEffects));
            ChildStates.Add("FatigueHealthEffects", new FatigueHealthEffectsSnippet(c.FatigueHealthEffects));
            ChildStates.Add("InventoryHealthEffects", new InventoryHealthEffectsSnippet(c.InventoryHealthEffects));
            ChildStates.Add("MedicalAgentsHealthEffects", new MedicalAgentsHealthEffectsSnippet(c.MedicalAgentsHealthEffects));
            ChildStates.Add("ConsumablesHealthEffects", new ConsumablesHealthEffectsSnippet(c.ConsumablesHealthEffects));
            ChildStates.Add("ClothesHealthEffects", new ClothesHealthEffectsSnippet(c.ClothesHealthEffects));
        }

    }

}