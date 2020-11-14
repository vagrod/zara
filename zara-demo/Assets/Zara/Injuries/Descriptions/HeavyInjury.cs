using ZaraEngine.Diseases;
using ZaraEngine.Injuries.Stages.Fluent;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Injuries
{
    public class HeavyInjury : InjuryBase
    {

        public HeavyInjury()
        {
            Name = "Heavy Injury";
            Stages = new[]
            {
                InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.InitialStage)
                    .NoDescription()
                    .BasicInjury()
                    .WillLastForHours(1)
                    .WillSelfHealInHours(1)
                    .Drains
                        .BloodPerSecond(0.001f)
                    .WillNotAffectControls()
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
