using System;
using System.Collections.Generic;
using ZaraEngine.Diseases.Stages.Fluent;
using ZaraEngine.Inventory;

namespace ZaraEngine.Diseases.Stages
{
    public class DiseaseStage 
    {
        public DiseaseLevels Level { get; set; }
        public TimeSpan StageDuration { get; set; }
        public TimeSpan? TargetVitalsTime { get; set; }
        public float? TargetBloodPressureTop { get; set; }
        public float? TargetBloodPressureBottom { get; set; }
        public float? TargetBodyTemperature { get; set; }
        public float? TargetHeartRate { get; set; }
        public float? WaterDrainPerSecond { get; set; }
        public float? FoodDrainPerSecond { get; set; }
        public float? StaminaDrainPerSecond { get; set; }
        public float? FatigueIncreasePerSecond { get; set; }
        public int BlackoutChance { get; set; }
        public int DizzinessChance { get; set; }
        public int CoughChance { get; set; }
        public int SneezeChance { get; set; }
        public int ChanceOfDeath { get; set; }
        public bool CannotSleep { get; set; }
        public bool CannotEat { get; set; }
        public bool CannotRun { get; set; }

        public int SelfHealChance { get; set; }

        public DateTime? WillTriggerAt { get; set; }
        public DateTime? WillEndAt { get; set; }
        public float? VitalsTargetSeconds { get; set; }

        public List<string> AcceptedTreatmentItems { get; set; }
        internal Func<IGameController, InventoryConsumableItemBase, ActiveDisease, bool> OnConsumeItem { get; set; }
        internal Func<IGameController, ApplianceInfo, ActiveDisease, bool> OnApplianceTaken { get; set; }
        internal Func<IGameController, ObjectDescriptionBase, ActiveDisease, bool> OnApplySpecialItem { get; set; }
    }
}
