using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries.Stages.Fluent;
using ZaraEngine.Injuries.Treatment;
using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Injuries
{
    public class Fracture : InjuryBase
    {

        private readonly ToolsOnlyInjuryTreatment _progressingStageTreatment;
        private readonly ToolsOnlyInjuryTreatment _worryingStageTreatment;
        private readonly ToolsOnlyInjuryTreatment _criticalStageTreatment;

        public Fracture()
        {
            _progressingStageTreatment = new ToolsOnlyInjuryTreatment(InventoryController.MedicalItems.BioactiveHydrogel, InventoryController.MedicalItems.Bandage, InventoryController.MedicalItems.Splint);
            _worryingStageTreatment = new ToolsOnlyInjuryTreatment(InventoryController.MedicalItems.BioactiveHydrogel, InventoryController.MedicalItems.Bandage, InventoryController.MedicalItems.Splint);
            _criticalStageTreatment = new ToolsOnlyInjuryTreatment(InventoryController.MedicalItems.BioactiveHydrogel, InventoryController.MedicalItems.DoripenemSyringe, InventoryController.MedicalItems.Bandage, InventoryController.MedicalItems.Splint);

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
                        .WithTreatmentAction(_progressingStageTreatment.OnApplianceTaken)
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
                        .WithTreatmentAction(_worryingStageTreatment.OnApplianceTaken)
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
                        .WithTreatmentAction(_criticalStageTreatment.OnApplianceTaken)
                    .Build()
            };
        }

        #region State Manage

        public override IStateSnippet GetState()
        {
            var state = new InjuryTreatmentSnippet();

            state.ToolsOnlyTreatments.Add((ToolsOnlyInjuryTreatmentSnippet)_progressingStageTreatment.GetState());
            state.ToolsOnlyTreatments.Add((ToolsOnlyInjuryTreatmentSnippet)_worryingStageTreatment.GetState());
            state.ToolsOnlyTreatments.Add((ToolsOnlyInjuryTreatmentSnippet)_criticalStageTreatment.GetState());

            return state;
        }

        public override void RestoreState(IStateSnippet savedState)
        {
            var state = (InjuryTreatmentSnippet)savedState;

            _progressingStageTreatment.RestoreState(state.ToolsOnlyTreatments[0]);
            _worryingStageTreatment.RestoreState(state.ToolsOnlyTreatments[1]);
            _criticalStageTreatment.RestoreState(state.ToolsOnlyTreatments[2]);
        }

        #endregion 

    }
}
