using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases.Stages.Fluent;

namespace ZaraEngine.Diseases.Stages
{
    public static class HealthyStageFactory
    {
        public static DiseaseStage Get(DiseaseLevels level, int gameHoursToReach)
        {
            return StageBuilder.NewStage()
                .WithLevelOfSeriousness(level)
                .NoSelfHeal()
                .Vitals
                    .WillReachTargetsInHours(gameHoursToReach)
                    .AndLastForHours(gameHoursToReach)
                    .WithTargetHeartRate(67)
                    .WithTargetBloodPressure(125, 74)
                    .WithTargetBodyTemperature(36.7f)
                .NoAdditionalEffects()
                .NoDisorders()
                .NoDrains()
                .Treatment
                    .WithoutConsumable()
                    .AndWithoutSpecialItems()
                .Build();
        }

    }
}
