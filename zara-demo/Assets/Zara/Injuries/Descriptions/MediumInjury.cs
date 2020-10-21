using ZaraEngine.Diseases;
using ZaraEngine.Injuries.Stages.Fluent;

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

    }
}
