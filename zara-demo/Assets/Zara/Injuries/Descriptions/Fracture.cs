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
    public class Fracture : InjuryBase
    {

        public Fracture()
        {
            var progressingStageTreatment = new ToolsOnlyInjuryTreatment(InventoryController.MedicalItems.BioactiveHydrogel, InventoryController.MedicalItems.Bandage, InventoryController.MedicalItems.Splint);
            var worryingStageTreatment = new ToolsOnlyInjuryTreatment(InventoryController.MedicalItems.BioactiveHydrogel, InventoryController.MedicalItems.Bandage, InventoryController.MedicalItems.Splint);
            var criticalStageTreatment = new ToolsOnlyInjuryTreatment(InventoryController.MedicalItems.BioactiveHydrogel, InventoryController.MedicalItems.DoripenemSyringe, InventoryController.MedicalItems.Bandage, InventoryController.MedicalItems.Splint);

            Name = "Fracture";
            Stages = new[]
            {
                InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Progressing)
                    .NoDescription()
                    .ClosedFracture()
                    .WillLastForHours(1)
                    .NoSelfHeal()
                    .NoDrains()
                    .WillNotBeAbleToRun()
                    .NoSpeedImpact()
                    .Treatment
                        .WithTreatmentAction(progressingStageTreatment.OnApplianceTaken)
                    .Build(),

                InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Worrying)
                    .NoDescription()
                    .ClosedFracture()
                    .WillLastForHours(1)
                    .NoSelfHeal()
                    .NoDrains()
                    .WillNotBeAbleToRun()
                    .NoSpeedImpact()
                    .Treatment
                        .WithTreatmentAction(worryingStageTreatment.OnApplianceTaken)
                    .Build(),

                 InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Critical)
                    .NoDescription()
                    .ClosedFracture()
                    .WillLastForever()
                    .TriggersDisease<BloodPoisoning>(10)
                    .NoDrains()
                    .WillNotBeAbleToRun()
                    .NoSpeedImpact()
                    .Treatment
                        .WithTreatmentAction(criticalStageTreatment.OnApplianceTaken)
                    .Build()
            };
        }

    }
}
