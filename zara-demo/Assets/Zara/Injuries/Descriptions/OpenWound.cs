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
    public class OpenWound : InjuryBase
    {

        public OpenWound()
        {
            var initialStageTreatment = new ToolsOnlyInjuryTreatment(MedicalAppliancesGroup.AntisepticGroup, InventoryController.MedicalItems.Bandage);
            var progressingStageTreatment = new ToolsOnlyInjuryTreatment(MedicalAppliancesGroup.AntisepticGroup, InventoryController.MedicalItems.Bandage);
            var worryingStageTreatment = new ToolsOnlyInjuryTreatment(InventoryController.MedicalItems.SuctionPump, MedicalAppliancesGroup.AntisepticGroup, InventoryController.MedicalItems.Bandage);

            Name = "Open Wound";
            Stages = new[]
            {
                InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.InitialStage)
                    .NoDescription()
                    .Cut()
                    .WillLastForHours(3)
                    .NoSelfHeal()
                    .Drains
                    .BloodPerSecond(0.0025f)
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
                    .TriggersDisease<BloodPoisoning>(62)
                    .Drains
                        .BloodPerSecond(0.0025f)
                    .WillNotAffectControls()
                    .Treatment
                        .WithTreatmentAction(worryingStageTreatment.OnApplianceTaken)
                    .Build()
            };
        }

    }
}
