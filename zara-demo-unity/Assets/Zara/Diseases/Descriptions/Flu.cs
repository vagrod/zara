using System.Collections.Generic;
using ZaraEngine.Diseases.Stages;
using ZaraEngine.Diseases.Stages.Fluent;
using ZaraEngine.Diseases.Treatment;
using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Diseases
{
    public class Flu : DiseaseDefinitionBase
    {
        private readonly ConsumableTimedTreatment _initialStageTreatment;
        private readonly ConsumableTimedTreatment _progressingStageTreatment;
        private readonly ConsumableTimedTreatmentNode _worryingStageTreatment;
        private readonly ConsumableTimedTreatmentNode _criticalingStageTreatment;

        public Flu()
        {
            // Describe treatment for each stage
            _initialStageTreatment = new ConsumableTimedTreatment(DiseaseLevels.InitialStage, MedicalConsumablesGroup.OseltamivirGroup);
            _progressingStageTreatment = new ConsumableTimedTreatment(DiseaseLevels.Progressing, MedicalConsumablesGroup.OseltamivirGroup, 60, 3);
            _worryingStageTreatment = new ConsumableTimedTreatmentNode(DiseaseLevels.Worrying,
                new ConsumableTimedTreatment(MedicalConsumablesGroup.OseltamivirGroup, 90, 3),
                new ConsumableTimedTreatment(MedicalConsumablesGroup.AntibioticGroup, 90, 3)
            );
            _criticalingStageTreatment = new ConsumableTimedTreatmentNode(DiseaseLevels.Critical,
                new ConsumableTimedTreatment(MedicalConsumablesGroup.OseltamivirGroup, 90, 4),
                new ConsumableTimedTreatment(MedicalConsumablesGroup.AntibioticGroup, 90, 4)
            );

            Name = "Flu";
            Stages = new List<DiseaseStage>(new[]
            {
                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.InitialStage)
                                             .SelfHealChance(20)
                                             .Vitals
                                                 .WithTargetBodyTemperature(37.1f)
                                             .WillReachTargetsInHours(1)
                                             .AndLastForHours(3)
                                             .AdditionalEffects
                                                 .WithLowChanceOfSneeze()
                                             .NoDisorders()
                                             .NoDrains()
                                             .Treatment
                                                 .WithConsumable(_initialStageTreatment.OnItemConsumed)
                                                 .AndWithoutSpecialItems()
                                             .Build(),

                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Progressing)
                                             .NoSelfHeal()
                                             .Vitals
                                                 .WithTargetBodyTemperature(37.9f)
                                             .WillReachTargetsInHours(2)
                                             .AndLastForHours(2).AndMinutes(35)
                                             .AdditionalEffects
                                                 .WithMediumChanceOfSneeze()
                                                 .WithLowChanceOfCough()
                                             .NoDisorders()
                                             .Drain
                                                 .FatigueIncreasePerSecond(0.0001f)
                                             .Treatment
                                                 .WithConsumable(_progressingStageTreatment.OnItemConsumed)
                                                 .AndWithoutSpecialItems()
                                             .Build(),

                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Worrying)
                                             .NoSelfHeal()
                                             .Vitals
                                                 .WithTargetBodyTemperature(38.7f)
                                             .WillReachTargetsInHours(2)
                                             .AndLastForHours(4).AndMinutes(30)
                                             .AdditionalEffects
                                                 .WithHighChanceOfSneeze()
                                                 .WithMediumChanceOfCough()
                                                 .WithLowChanceOfDizziness()
                                             .NoDisorders()
                                             .Drain
                                                 .WaterPerSecond(0.005f)
                                                 .FatigueIncreasePerSecond(0.0002f)
                                             .Treatment
                                                 .WithConsumable(_worryingStageTreatment.OnItemConsumed)
                                                 .AndWithoutSpecialItems()
                                             .Build(),

                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Critical)
                                             .NoSelfHeal()
                                             .Vitals
                                                 .WithTargetBodyTemperature(41.2f)
                                             .WillReachTargetsInHours(2)
                                             .AndLastUntilEnd()
                                             .AdditionalEffects
                                                 .WithHighChanceOfCough()
                                                 .WithMediumChanceOfDizziness()
                                                 .WithLowChanceOfBlackouts()
                                             .Disorders
                                                 .WillNotBeAbleToRun()
                                                 .NotDeadly()
                                             .Drain
                                                 .WaterPerSecond(0.01f)
                                                 .FoodPerSecond(0.005f)
                                                 .FatigueIncreasePerSecond(0.00035f)
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
