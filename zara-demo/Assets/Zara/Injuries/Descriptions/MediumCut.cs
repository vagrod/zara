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
    public class MediumCut : InjuryBase
    {

        public MediumCut()
        {
            var initialStageTreatment = new ToolsOnlyInjuryTreatment(MedicalAppliancesGroup.AntisepticGroup, InventoryController.MedicalItems.Bandage);
            var progressingStageTreatment = new ToolsOnlyInjuryTreatment(MedicalAppliancesGroup.AntisepticGroup, InventoryController.MedicalItems.Bandage);
            var worryingStageTreatment = new ToolsOnlyInjuryTreatment(InventoryController.MedicalItems.SuctionPump, MedicalAppliancesGroup.AntisepticGroup, InventoryController.MedicalItems.Bandage);

            Name = "Medium Cut";
            Stages = new[]
            {
                InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.InitialStage)
                    .NoDescription()
                    .Cut()
                    .WillLastForHours(4)
                    .NoSelfHeal()
                    .Drains
                        .BloodPerSecond(0.0015f)
                        .WillNotAffectControls()
                    .Treatment
                        .WithTreatmentAction(initialStageTreatment.OnApplianceTaken)
                    .Build(),

                InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Progressing)
                    .NoDescription()
                    .Cut()
                    .WillLastForHours(4)
                    .NoSelfHeal()
                    .Drains
                        .BloodPerSecond(0.0018f)
                        .WillNotAffectControls()
                    .Treatment
                        .WithTreatmentAction(progressingStageTreatment.OnApplianceTaken)
                    .Build(),

                InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.InitialStage)
                    .NoDescription()
                    .Cut()
                    .WillLastForHours(400)
                    .NoSelfHeal()
                    .Drains
                        .BloodPerSecond(0.001f)
                        .WillNotAffectControls()
                    .Treatment
                        .WithTreatmentAction(worryingStageTreatment.OnApplianceTaken)
                    .Build()
            };
        }

    }
}
