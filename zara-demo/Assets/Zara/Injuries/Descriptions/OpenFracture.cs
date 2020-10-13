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
    public class OpenFracture : InjuryBase
    {

        public OpenFracture()
        {
            var worryingStageTreatment = new ToolsOnlyInjuryTreatment(InventoryController.MedicalItems.BioactiveHydrogel, InventoryController.MedicalItems.DoripenemSyringe, InventoryController.MedicalItems.Bandage, InventoryController.MedicalItems.Splint);
            var criticalStageTreatment = new ToolsOnlyInjuryTreatment(InventoryController.MedicalItems.BioactiveHydrogel, InventoryController.MedicalItems.DoripenemSyringe, InventoryController.MedicalItems.Bandage, InventoryController.MedicalItems.Splint);

            Name = "Open Fracture";
            Stages = new[]
            {
                InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Worrying)
                    .NoDescription()
                    .OpenFracture()
                    .WillLastForHours(1)
                    .NoSelfHeal()
                    .Drains
                        .BloodPerSecond(0.001f)
                    .WillAffectControls()
                        .WillNotBeAbleToRun()
                        .NoSpeedImpact()
                    .Treatment
                        .WithTreatmentAction(worryingStageTreatment.OnApplianceTaken)
                    .Build(),

                 InjuryStageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Critical)
                    .NoDescription()
                    .OpenFracture()
                    .WillLastForever()
                    .TriggersDisease<BloodPoisoning>(35)
                    .Drains
                        .BloodPerSecond(0.001f)
                     .WillAffectControls()
                        .WillNotBeAbleToRun()
                        .NoSpeedImpact()
                    .Treatment
                        .WithTreatmentAction(criticalStageTreatment.OnApplianceTaken)
                    .Build()
            };
        }

    }
}