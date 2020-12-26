using System.Collections.Generic;
using ZaraEngine.Diseases.Stages;
using ZaraEngine.Diseases.Stages.Fluent;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Diseases
{
    public class Hyperthermia : DiseaseDefinitionBase
    {

        public Hyperthermia()
        {
            Name = "Hyperthermia";

            Stages = new List<DiseaseStage>(new[]
            {
                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.InitialStage)
                    .NoSelfHeal()
                    .Vitals
                        .WithTargetBodyTemperature(37.5f)
                        .WithTargetHeartRate(76)
                    .WillReachTargetsInHours(1)
                    .AndLastForHours(1)
                    .AdditionalEffects
                        .WithLowAdditionalStaminaDrain()
                    .NoDisorders()
                    .NoDrains()
                    .Treatment
                        .WithoutConsumable()
                        .AndWithoutSpecialItems()
                    .Build(),

                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Worrying)
                    .NoSelfHeal()
                    .Vitals
                        .WithTargetBodyTemperature(38.3f)
                        .WithTargetHeartRate(85)
                        .WithTargetBloodPressure(132f, 78)
                    .WillReachTargetsInHours(1)
                    .AndLastForHours(1)
                    .AdditionalEffects
                        .WithMediumAdditionalStaminaDrain()
                    .Disorders
                        .NotDeadly()
                    .Drain
                        .FatigueIncreasePerSecond(0.0078f)
                    .Treatment
                        .WithoutConsumable()
                        .AndWithoutSpecialItems()
                    .Build(),

                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Critical)
                    .NoSelfHeal()
                    .Vitals
                        .WithTargetBodyTemperature(39.9f)
                        .WithTargetHeartRate(108)
                        .WithTargetBloodPressure(152f, 101f)
                    .WillReachTargetsInHours(1)
                    .AndLastUntilEnd()
                    .AdditionalEffects
                        .WithMediumAdditionalStaminaDrain()
                    .Disorders
                        .WillNotBeAbleToRun()
                        .WithFoodDisgust()
                        .NotDeadly()
                    .Drain
                        .FatigueIncreasePerSecond(0.0106f)
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

            //...
        }

        #endregion 
    }
}
