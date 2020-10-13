using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries.Stages.Fluent;

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

    }
}
