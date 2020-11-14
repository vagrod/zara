using ZaraEngine.Diseases;
using ZaraEngine.Injuries.Stages.Fluent;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Injuries
{
    public class MediumInjury : InjuryBase
    {

        public MediumInjury()
        {
            Name = "Medium Injury";
            Stages = new[]
            {
                InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.InitialStage)
                    .NoDescription()
                    .BasicInjury()
                    .WillLastForHours(8)
                    .WillSelfHealInHours(8)
                    .NoDrains()
                    .NoSpeedImpact()
                    .NoTreatment()
                    .Build()
            };
        }

        #region State Manage

        public override IStateSnippet GetState()
        {
            var state = new InjuryTreatmentSnippet();

            return state;
        }

        public override void RestoreState(IStateSnippet savedState)
        {

        }

        #endregion 

    }
}
