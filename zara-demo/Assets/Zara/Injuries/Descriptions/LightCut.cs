using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries.Stages.Fluent;

namespace ZaraEngine.Injuries
{
    public class LightCut : InjuryBase
    {

        public LightCut()
        {
            Name = "Light Cut";
            Stages = new[]
            {
                InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.InitialStage)
                    .NoDescription()
                    .Cut()
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
