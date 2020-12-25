using System;
using System.Linq;
using ZaraEngine.Inventory;

namespace ZaraEngine.Diseases.Stages.Fluent
{
    public class StageBuilder : IStageDisorder, IStageFinish, IStageTreatmentNode, IStageTreatmentConsumableAction, IStageSelfHeal, IStageTreatmentItemAction, IStageTreatmentItems, IStageLevel, IStageVitals, IStageAdditional, IStageDrains, IStageDrainsNode, IStageVitalsAndDuration, IStageDurationTiming, IStageVitalsNode
    {

        private readonly DiseaseStage _obj;

        private const int ChanceLow = 7;

        private const int ChanceMedium = 33;

        private const int ChanceHigh = 65;

        public StageBuilder()
        {
            _obj = new DiseaseStage();
        }

        public static IStageLevel NewStage()
        {
            return new StageBuilder();
        }

        public IStageDisorder WithSleepDisorder()
        {
            _obj.CannotSleep = true;

            return this;
        }

        public IStageDisorder WithFoodDisgust()
        {
            _obj.CannotEat = true;

            return this;
        }

        public IStageDisorder WillNotBeAbleToRun()
        {
            _obj.CannotRun = true;

            return this;
        }

        public IStageDrainsNode NotDeadly()
        {
            _obj.ChanceOfDeath = 0;

            return this;
        }

        public IStageDrainsNode WithLowRiskOfDeath()
        {
            _obj.ChanceOfDeath = ChanceLow;

            return this;
        }

        public IStageDrainsNode WithMediumRiskOfDeath()
        {
            _obj.ChanceOfDeath = ChanceMedium;

            return this;
        }

        public IStageDrainsNode WithHighRiskOfDeath()
        {
            _obj.ChanceOfDeath = ChanceHigh;

            return this;
        }

        public IStageDrainsNode WithCustomRiskOfDeath(int percentChance)
        {
            _obj.ChanceOfDeath = percentChance;

            return this;
        }

        public IStageVitalsAndDuration AndLastForHours(int value)
        {
            _obj.StageDuration = TimeSpan.FromHours(value);

            return this;
        }

        public IStageVitals AndMinutes(int value)
        {
            _obj.StageDuration = _obj.StageDuration.Add(TimeSpan.FromMinutes(value));
            _obj.TargetVitalsTime = _obj.StageDuration;

            return this;
        }

        public IStageVitalsAndDuration AndLastForMinutes(int value)
        {
            _obj.StageDuration = TimeSpan.FromMinutes(value);
            _obj.TargetVitalsTime = _obj.StageDuration;

            return this;
        }

        public IStageVitalsAndDuration AndLastUntilEnd()
        {
            _obj.StageDuration = TimeSpan.FromDays(1000);

            return this;
        }

        public DiseaseStage Build()
        {
            return _obj;
        }

        public IStageSelfHeal WithLevelOfSeriousness(DiseaseLevels value)
        {
            _obj.Level = value;

            return this;
        }

        public IStageVitals WithTargetBodyTemperature(float value)
        {
            _obj.TargetBodyTemperature = value;

            return this;
        }

        public IStageVitals WithTargetBloodPressure(float first, float second)
        {
            _obj.TargetBloodPressureTop = first;
            _obj.TargetBloodPressureBottom = second;

            return this;
        }

        public IStageVitals WithTargetHeartRate(float value)
        {
            _obj.TargetHeartRate = value;

            return this;
        }

        public IStageAdditional NoAdditionalEffects()
        {
            return this;
        }

        public IStageAdditional AdditionalEffects
        {
            get { return this; }
        }

        public IStageDurationTiming WillReachTargetsInHours(int value)
        {
            _obj.TargetVitalsTime = TimeSpan.FromHours(value);

            return this;
        }

        public IStageAdditional WithLowChanceOfBlackouts()
        {
            _obj.BlackoutChance = ChanceLow;

            return this;
        }

        public IStageAdditional WithMediumChanceOfBlackouts()
        {
            _obj.BlackoutChance = ChanceMedium;

            return this;
        }

        public IStageAdditional WithHighChanceOfBlackouts()
        {
            _obj.BlackoutChance = ChanceHigh;

            return this;
        }

        public IStageAdditional WithCustomChanceOfBlackouts(int chancePercent)
        {
            _obj.BlackoutChance = chancePercent;

            return this;
        }

        public IStageAdditional WithLowChanceOfDizziness()
        {
            _obj.DizzinessChance = ChanceLow;

            return this;
        }

        public IStageAdditional WithMediumChanceOfDizziness()
        {
            _obj.DizzinessChance = ChanceMedium;

            return this;
        }

        public IStageAdditional WithHighChanceOfDizziness()
        {
            _obj.DizzinessChance = ChanceHigh;

            return this;
        }

        public IStageAdditional WithCustomChanceOfDizziness(int chancePercent)
        {
            _obj.DizzinessChance = chancePercent;

            return this;
        }

        public IStageAdditional WithLowChanceOfCough()
        {
            _obj.CoughChance = ChanceLow;

            return this;
        }

        public IStageAdditional WithMediumChanceOfCough()
        {
            _obj.CoughChance = ChanceMedium;

            return this;
        }

        public IStageAdditional WithHighChanceOfCough()
        {
            _obj.CoughChance = ChanceHigh;

            return this;
        }

        public IStageAdditional WithCustomChanceOfCough(int chancePercent)
        {
            _obj.CoughChance = chancePercent;

            return this;
        }

        public IStageAdditional WithLowChanceOfSneeze()
        {
            _obj.SneezeChance = ChanceLow;

            return this;
        }

        public IStageAdditional WithMediumChanceOfSneeze()
        {
            _obj.SneezeChance = ChanceMedium;

            return this;
        }

        public IStageAdditional WithHighChanceOfSneeze()
        {
            _obj.SneezeChance = ChanceHigh;

            return this;
        }

        public IStageAdditional WithCustomChanceOfSneeze(int chancePercent)
        {
            _obj.SneezeChance = chancePercent;

            return this;
        }

        public IStageAdditional WithLowAdditionalStaminaDrain()
        {
            _obj.StaminaDrainPerSecond = 0.03f;

            return this;
        }

        public IStageAdditional WithMediumAdditionalStaminaDrain()
        {
            _obj.StaminaDrainPerSecond = 0.05f;

            return this;
        }

        public IStageAdditional WithHighAdditionalStaminaDrain()
        {
            _obj.StaminaDrainPerSecond = 0.1f;

            return this;
        }

        public IStageAdditional WithCustomAdditionalStaminaDrain(float drainValue)
        {
            _obj.StaminaDrainPerSecond = drainValue;

            return this;
        }

        public IStageDisorder Disorders { get { return this; } }

        public IStageDrainsNode NoDisorders()
        {
            return this;
        }

        public IStageVitals Vitals {
            get { return this; }
        }

        public IStageDrains WaterPerSecond(float value)
        {
            _obj.WaterDrainPerSecond = value;

            return this;
        }

        public IStageDrains FoodPerSecond(float value)
        {
            _obj.FoodDrainPerSecond = value;

            return this;
        }

        public IStageDrains FatigueIncreasePerSecond(float value)
        {
            _obj.FatigueIncreasePerSecond = value;

            return this;
        }

        public IStageDrains Drain
        {
            get { return this; }
        }

        public IStageTreatmentNode NoDrains()
        {
            return this;
        }

        public IStageTreatmentConsumableAction Treatment
        {
            get { return this; }
        }

        public IStageTreatmentItems WithConsumable(Func<IGameController, InventoryConsumableItemBase, ActiveDisease, bool> treatmentAction)
        {
            _obj.OnConsumeItem = treatmentAction;

            return this;
        }

        public IStageTreatmentItems WithAppliance(Func<IGameController, ApplianceInfo, ActiveDisease, bool> treatmentAction)
        {
            _obj.OnApplianceTaken = treatmentAction;

            return this;
        }

        public IStageTreatmentItems WithoutConsumable()
        {
            return this;
        }

        public IStageFinish WithTreatmentAction(Func<IGameController, ObjectDescriptionBase, ActiveDisease, bool> treatmentAction)
        {
            _obj.OnApplySpecialItem = treatmentAction;

            return this;
        }


        public IStageTreatmentItemAction AndWithSpecialItems(params string[] items)
        {
            _obj.AcceptedTreatmentItems = items.ToList();

            return this;
        }

        public IStageFinish AndWithoutSpecialItems()
        {
            return this;
        }

        public IStageVitalsNode SelfHealChance(int percent)
        {
            _obj.SelfHealChance = percent;

            return this;
        }

        public IStageVitalsNode NoSelfHeal()
        {
            return this;
        }
    }
}
