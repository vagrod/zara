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

        public HealthStateStateContract Status;

        public UnderwaterHealthEffectsContract UnderwaterHealthEffects;
        public RunningHealthEffectsContract RunningHealthEffects;
        public FatigueHealthEffectsContract FatigueHealthEffects;
        public InventoryHealthEffectsContract InventoryHealthEffects;
        public MedicalAgentsHealthEffectsContract MedicalAgentsHealthEffects;
        public ConsumablesHealthEffectsContract ConsumablesHealthEffects;
        public ClothesHealthEffectsContract ClothesHealthEffects;

        public DiseaseMonitorsContract DiseaseMonitors;
        public ActiveMedicalAgentsMonitorsContract ActiveMedicalAgentsMonitors;

        public EventByChanceContract DiseaseDizzinessEvent;
        public EventByChanceContract DiseaseBlackoutsEvent;
        public EventByChanceContract DiseaseDeathEvent;
        public EventByChanceContract SneezeEvent;
        public EventByChanceContract CoughEvent;
        public EventByChanceContract BloodLevelDizzinessEvent;
        public EventByChanceContract BloodLevelBlackoutsEvent;
        public EventByChanceContract LowBodyTemperatureDizzinessEvent;
        public EventByChanceContract LowBodyTemperatureBlackoutsEvent;
        public EventByChanceContract BloodLevelDeathEvent;
        public EventByChanceContract DehydrationDeathEvent;
        public EventByChanceContract StarvationDeathEvent;
        public EventByChanceContract VitalsDeathEvent;
        public EventByChanceContract OverdoseDeathEvent;
        public EventByChanceContract HeartFailureDeathEvent;
        public EventByChanceContract LsdEffect;
        public EventByChanceContract FatigueDizzinessEvent;
        public EventByChanceContract FatigueBlackoutsEvent;
        public EventByChanceContract FatigueSleepEvent;
        public EventByChanceContract SedativeSleepEvent;

        public FixedEventContract HighPressureEvent;
        public FixedEventContract NormalPressureEvent;
        public FixedEventContract DrowningEvent;

    }

}