using System.Collections.Generic;
using ZaraEngine.Diseases.Stages;
using ZaraEngine.Diseases.Stages.Fluent;
using ZaraEngine.Diseases.Treatment;
using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Diseases
{
    public class FoodPoisoning : DiseaseDefinitionBase
    {

        private readonly ConsumableTimedTreatment _initialStageTreatment;
        private readonly ConsumableTimedTreatment _progressingStageTreatment;
        private readonly ConsumableTimedTreatmentNode _worryingStageTreatment;
        private readonly ConsumableTimedTreatmentNode _criticalingStageTreatment;

        public FoodPoisoning()
        {
            // Describe treatment for each stage
            _initialStageTreatment = new ConsumableTimedTreatment(DiseaseLevels.InitialStage, MedicalConsumablesGroup.LoperamideGroup);
            _progressingStageTreatment = new ConsumableTimedTreatment(DiseaseLevels.Progressing, MedicalConsumablesGroup.LoperamideGroup, 90, 2);
            _worryingStageTreatment = new ConsumableTimedTreatmentNode(DiseaseLevels.Worrying,
                new ConsumableTimedTreatment(MedicalConsumablesGroup.LoperamideGroup, 90, 3),
                new ConsumableTimedTreatment(MedicalConsumablesGroup.AntibioticGroup, 60, 3)
            );
            _criticalingStageTreatment = new ConsumableTimedTreatmentNode(DiseaseLevels.Critical,
                new ConsumableTimedTreatment(MedicalConsumablesGroup.LoperamideGroup, 90, 4),
                new ConsumableTimedTreatment(MedicalConsumablesGroup.AntibioticGroup, 60, 4)
            );

            Name = "FoodPoisoning";
            Stages = new List<DiseaseStage>(new[]
            {
                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.InitialStage)
                                             .SelfHealChance(5)
                                             .Vitals
                                                 .WithTargetBodyTemperature(36.9f)
                                             .WillReachTargetsInHours(1)
                                             .AndLastForHours(3)
                                             .NoAdditionalEffects()
                                             .NoDisorders()
                                             .NoDrains()
                                             .Treatment
                                                 .WithConsumable(_initialStageTreatment.OnItemConsumed)
                                                 .AndWithoutSpecialItems()
                                             .Build(),

                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Progressing)
                                             .NoSelfHeal()
                                             .Vitals
                                                 .WithTargetBodyTemperature(37.2f)
                                             .WillReachTargetsInHours(1)
                                             .AndLastForHours(1).AndMinutes(35)
                                             .NoAdditionalEffects()
                                             .NoDisorders()
                                             .Drain
                                                 .FatigueIncreasePerSecond(0.00005f)
                                             .Treatment
                                                 .WithConsumable(_progressingStageTreatment.OnItemConsumed)
                                                 .AndWithoutSpecialItems()
                                             .Build(),

                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Worrying)
                                             .NoSelfHeal()
                                             .Vitals
                                                 .WithTargetBodyTemperature(37.9f)
                                             .WillReachTargetsInHours(2)
                                             .AndLastForHours(2).AndMinutes(30)
                                             .AdditionalEffects
                                             .Disorders
                                                 .WithFoodDisgust()
                                                 .NotDeadly()
                                             .Drain
                                                 .WaterPerSecond(0.0025f)
                                                 .FatigueIncreasePerSecond(0.0001f)
                                             .Treatment
                                                 .WithConsumable(_worryingStageTreatment.OnItemConsumed)
                                                 .AndWithoutSpecialItems()
                                             .Build(),

                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Critical)
                                             .NoSelfHeal()
                                             .Vitals
                                                 .WithTargetBodyTemperature(38.6f)
                                             .WillReachTargetsInHours(2)
                                             .AndLastUntilEnd()
                                             .NoAdditionalEffects()
                                             .Disorders
                                                 .WithFoodDisgust()
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
