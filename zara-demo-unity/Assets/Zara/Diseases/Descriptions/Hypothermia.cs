using System.Collections.Generic;
using ZaraEngine.Diseases.Stages;
using ZaraEngine.Diseases.Stages.Fluent;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Diseases
{
    // No treatment here. Hypothermia is controller totally from the <see cref="HypothermiaMonitor"/>
    public class Hypothermia : DiseaseDefinitionBase
    {

        public Hypothermia()
        {
            Name = "Hypothermia";

            Stages = new List<DiseaseStage>(new[]
            {
                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.InitialStage)
                    .NoSelfHeal()
                    .Vitals
                        .WithTargetBodyTemperature(35.9f)
                        .WithTargetHeartRate(52)
                    .WillReachTargetsInHours(1)
                    .AndLastForHours(1)
                    .AdditionalEffects
                        .WithLowAdditionalStaminaDrain()
                    .NoDisorders()
                    .Drain
                        .FatigueIncreasePerSecond(0.0035f)
                    .Treatment
                        .WithoutConsumable()
                        .AndWithoutSpecialItems()
                    .Build(),

                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Worrying)
                    .NoSelfHeal()
                    .Vitals
                        .WithTargetBodyTemperature(35.1f)
                        .WithTargetHeartRate(41)
                    .WillReachTargetsInHours(1)
                    .AndLastForHours(1)
                    .AdditionalEffects
                        .WithMediumAdditionalStaminaDrain()
                    .Disorders
                        .WillNotBeAbleToRun()
                        .NotDeadly()
                    .Drain
                        .FatigueIncreasePerSecond(0.0124f)
                    .Treatment
                        .WithoutConsumable()
                        .AndWithoutSpecialItems()
                    .Build(),

                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Critical)
                    .NoSelfHeal()
                    .Vitals
                        .WithTargetBodyTemperature(33.4f)
                        .WithTargetHeartRate(33)
                        .WithTargetBloodPressure(91f, 52f)
                    .WillReachTargetsInHours(1)
                    .AndLastUntilEnd()
                    .AdditionalEffects
                        .WithMediumAdditionalStaminaDrain()
                    .Disorders
                        .WillNotBeAbleToRun()
                        .WithFoodDisgust()
                        .NotDeadly()
                    .Drain
                        .FatigueIncreasePerSecond(0.0328f)
                    .Treatment
                        .WithoutConsumable()
                        .AndWithoutSpecialItems()
                    .Build()
            });
        }

        #region State Manage

        public override IStateSnippet GetState()
        {
            var state = new DiseaseTreatmentSnippet();

            return state;
        }

        public override void RestoreState(IStateSnippet savedState)
        {
            var state = (DiseaseTreatmentSnippet)savedState;

            // Nothing to store really
        }

        #endregion 

    }
}
