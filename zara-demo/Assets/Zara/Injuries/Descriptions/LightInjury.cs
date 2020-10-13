using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries.Stages.Fluent;

namespace ZaraEngine.Injuries
{
    public class LightInjury : InjuryBase
    {

        public LightInjury()
        {
            Name = "Light Injury";
            Stages = new[]
            {
                InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.InitialStage)
                    .NoDescription()
                    .BasicInjury()
                    .WillLastForHours(3)
                    .WillSelfHealInHours(3)
                    .NoDrains()
                    .NoSpeedImpact()
                    .NoTreatment()
                    .Build()
            };
        }

    }
}
