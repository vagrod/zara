using System.Collections.Generic;
using ZaraEngine.Diseases.Stages;
using ZaraEngine.Diseases.Stages.Fluent;
using ZaraEngine.Diseases.Treatment;
using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Diseases
{
    public class Angina : DiseaseDefinitionBase
    {

        private readonly ConsumableTimedTreatment _initialStageTreatment;
        private readonly ConsumableTimedTreatment _progressingStageTreatment;
        private readonly ConsumableTimedTreatmentNode _worryingStageTreatment;
        private readonly ConsumableTimedTreatmentNode _criticalingStageTreatment;

        public Angina()
        {
            // Describe treatment for each stage
            _initialStageTreatment = new ConsumableTimedTreatment(DiseaseLevels.InitialStage, MedicalConsumablesGroup.AcetaminophenGroup, 90, 2);
            _progressingStageTreatment = new ConsumableTimedTreatment(DiseaseLevels.Progressing, MedicalConsumablesGroup.AcetaminophenGroup, 60, 3);
            _worryingStageTreatment = new ConsumableTimedTreatmentNode(DiseaseLevels.Worrying,
                new ConsumableTimedTreatment(MedicalConsumablesGroup.AcetaminophenGroup, 60, 3),
                new ConsumableTimedTreatment(MedicalConsumablesGroup.AntibioticGroup, 60, 3)
            );
            _criticalingStageTreatment = new ConsumableTimedTreatmentNode(DiseaseLevels.Critical,
                new ConsumableTimedTreatment(MedicalConsumablesGroup.AcetaminophenGroup, 60, 4),
                new ConsumableTimedTreatment(MedicalConsumablesGroup.AntibioticGroup, 60, 4)
            );

            Name = "Angina";
            Stages = new List<DiseaseStage> (new []
            {
                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.InitialStage)
                                             .NoSelfHeal()
                                             .Vitals
                                                 .WithTargetBodyTemperature(38.0f)
                                             .WillReachTargetsInHours(2)
                                             .AndLastForHours(3)
                                             .AdditionalEffects
                                                 .WithLowChanceOfCough()
                                             .NoDisorders()
                                             .Drain
                                                 .FatigueIncreasePerSecond(0.0015f)
                                             .Treatment
                                                 .WithConsumable(_initialStageTreatment.OnItemConsumed)
                                                 .AndWithoutSpecialItems()
                                             .Build(),

                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Progressing)
                                             .NoSelfHeal()
                                             .Vitals
                                                 .WithTargetBodyTemperature(38.9f)
                                             .WillReachTargetsInHours(2)
                                             .AndLastForHours(4).AndMinutes(40)
                                             .AdditionalEffects
                                                 .WithMediumChanceOfCough()
                                             .NoDisorders()
                                             .Drain
                                                 .FatigueIncreasePerSecond(0.025f)
                                             .Treatment
                                                 .WithConsumable(_progressingStageTreatment.OnItemConsumed)
                                                .AndWithoutSpecialItems()
                                             .Build(),

                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Worrying)
                                             .NoSelfHeal()
                                             .Vitals
                                                 .WithTargetBodyTemperature(39.1f)
                                                 .WithTargetBloodPressure(182, 120)
                                                 .WithTargetHeartRate(92)
                                             .WillReachTargetsInHours(1)
                                             .AndLastForHours(4).AndMinutes(30)
                                             .AdditionalEffects
                                                 .WithHighChanceOfCough()
                                                 .WithLowChanceOfDizziness()
                                             .Disorders
                                                 .WithFoodDisgust()
                                                 .NotDeadly()
                                             .Drain
                                                 .FoodPerSecond(0.007f)
                                                 .WaterPerSecond(0.005f)
                                                 .FatigueIncreasePerSecond(0.0035f)
                                             .Treatment
                                                .WithConsumable(_worryingStageTreatment.OnItemConsumed)
                                                .AndWithoutSpecialItems()
                                             .Build(),

                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Critical)
                                             .NoSelfHeal()
                                             .Vitals
                                                 .WithTargetBodyTemperature(41.1f)
                                                 .WithTargetBloodPressure(203, 180)
                                                 .WithTargetHeartRate(130)
                                             .WillReachTargetsInHours(2)
                                             .AndLastUntilEnd()
                                             .AdditionalEffects
                                                 .WithHighChanceOfCough()
                                                 .WithHighChanceOfDizziness()
                                                 .WithMediumChanceOfBlackouts()
                                             .Disorders
                                                 .WithFoodDisgust()
                                                 .WithSleepDisorder()
                                                 .WillNotBeAbleToRun()
                                                 .NotDeadly()
                                             .Drain
                                                 .FoodPerSecond(0.05f)
                                                 .WaterPerSecond(0.015f)
                                                 .FatigueIncreasePerSecond(0.0042f)
                                             .Treatment
                                                 .WithConsumable(_criticalingStageTreatment.OnItemConsumed)
                                                 .AndWithoutSpecialItems()
                                             .Build()
            });
        }

        public override void OnResumeDisease()
        {
            _initialStageTreatment.Reset();
            _progressingStageTreatment.Reset();
            _worryingStageTreatment.Reset();
            _criticalingStageTreatment.Reset();
        }

        public override void Check(ActiveDisease disease, IGameController gc)
        {
            _initialStageTreatment.Check(disease, gc);
            _progressingStageTreatment.Check(disease, gc);
            _worryingStageTreatment.Check(disease, gc);
            _criticalingStageTreatment.Check(disease, gc);
        }

        #region State Manage

        public override IStateSnippet GetState()
        {
            var state = new DiseaseTreatmentSnippet();

            state.ConsumableTimedTreatments.Add((ConsumableTimedTreatmentSnippet)_initialStageTreatment.GetState());
            state.ConsumableTimedTreatments.Add((ConsumableTimedTreatmentSnippet)_progressingStageTreatment.GetState());
            state.ConsumableTimedTreatmentNodes.Add((ConsumableTimedTreatmentNodeSnippet)_worryingStageTreatment.GetState());
            state.ConsumableTimedTreatmentNodes.Add((ConsumableTimedTreatmentNodeSnippet)_criticalingStageTreatment.GetState());

            return state;
        }

        public override void RestoreState(IStateSnippet savedState)
        {
            var state = (DiseaseTreatmentSnippet)savedState;

            _initialStageTreatment.RestoreState(state.ConsumableTimedTreatments[0]);
            _progressingStageTreatment.RestoreState(state.ConsumableTimedTreatments[1]);
            _worryingStageTreatment.RestoreState(state.ConsumableTimedTreatmentNodes[0]);
            _criticalingStageTreatment.RestoreState(state.ConsumableTimedTreatmentNodes[1]);
        }

        #endregion 

    }
}
