using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases.Stages;
using ZaraEngine.Diseases.Stages.Fluent;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Diseases
{
    public class Hypothermia : DiseaseDefinitionBase
    {

        public Hypothermia()
        {
            Name = "Hypothermia";
            IsDynamic = true;

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
                    .WithConsumable((gc, consumable, disease) =>
                    {
                        return false;
                    })
                    .AndWithoutSpecialItems()
                    .Build()
            });
        }

        public override void Check(ActiveDisease disease, IGameController gc)
        {

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
