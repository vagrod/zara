using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries.Stages.Fluent;
using ZaraEngine.Injuries.Treatment;
using ZaraEngine.Inventory;

namespace ZaraEngine.Injuries
{
    public class DeepCut : InjuryBase
    {
        public DeepCut()
        {
            var initialStageTreatment = new ToolsOnlyInjuryTreatment(MedicalAppliancesGroup.AntisepticGroup, InventoryController.MedicalItems.Bandage);
            var progressingStageTreatment = new ToolsOnlyInjuryTreatment(MedicalAppliancesGroup.AntisepticGroup, InventoryController.MedicalItems.Bandage);
            var worryingStageTreatment = new ToolsOnlyInjuryTreatment(InventoryController.MedicalItems.SuctionPump, MedicalAppliancesGroup.AntisepticGroup, InventoryController.MedicalItems.Bandage);

            Name = "Deep Cut";
            Stages = new[]
            {
                InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.InitialStage)
                    .NoDescription()
                    .Cut()
                    .WillLastForHours(3)
                    .NoSelfHeal()
                    .Drains
                        .BloodPerSecond(0.002f)
                    .WillNotAffectControls()
                    .Treatment
                        .WithTreatmentAction(initialStageTreatment.OnApplianceTaken)
                    .Build(),

                InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Progressing)
                    .NoDescription()
                    .Cut()
                    .WillLastForHours(3)
                    .NoSelfHeal()
                    .Drains
                        .BloodPerSecond(0.002f)
                    .WillNotAffectControls()
                    .Treatment
                        .WithTreatmentAction(progressingStageTreatment.OnApplianceTaken)
                    .Build(),

                InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Worrying)
                    .NoDescription()
                    .Cut()
                    .WillLastForever()
                    .TriggersDisease<BloodPoisoning>(35)
                    .Drains
                        .BloodPerSecond(0.0024f)
                    .WillNotAffectControls()
                    .Treatment
                        .WithTreatmentAction(worryingStageTreatment.OnApplianceTaken)
                    
                    .Build()
            };
        }

    }
}
