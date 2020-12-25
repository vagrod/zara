using ZaraEngine.Diseases;
using ZaraEngine.Injuries.Stages.Fluent;
using ZaraEngine.Injuries.Treatment;
using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Injuries
{
    public class OpenFracture : InjuryBase
    {

        private readonly ToolsOnlyInjuryTreatment _worryingStageTreatment;
        private readonly ToolsOnlyInjuryTreatment _criticalStageTreatment;

        public OpenFracture()
        {
            _worryingStageTreatment = new ToolsOnlyInjuryTreatment(InventoryController.MedicalItems.BioactiveHydrogel, InventoryController.MedicalItems.DoripenemSyringe, InventoryController.MedicalItems.Bandage, InventoryController.MedicalItems.Splint);
            _criticalStageTreatment = new ToolsOnlyInjuryTreatment(InventoryController.MedicalItems.BioactiveHydrogel, InventoryController.MedicalItems.DoripenemSyringe, InventoryController.MedicalItems.Bandage, InventoryController.MedicalItems.Splint);

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
                        .WithTreatmentAction(_worryingStageTreatment.OnApplianceTaken)
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
                        .WithTreatmentAction(_criticalStageTreatment.OnApplianceTaken)
                    .Build()
            };
        }

        #region State Manage

        public override IStateSnippet GetState()
        {
            var state = new InjuryTreatmentSnippet();

            state.ToolsOnlyTreatments.Add((ToolsOnlyInjuryTreatmentSnippet)_worryingStageTreatment.GetState());
            state.ToolsOnlyTreatments.Add((ToolsOnlyInjuryTreatmentSnippet)_criticalStageTreatment.GetState());

            return state;
        }

        public override void RestoreState(IStateSnippet savedState)
        {
            var state = (InjuryTreatmentSnippet)savedState;

            _worryingStageTreatment.RestoreState(state.ToolsOnlyTreatments[0]);
            _criticalStageTreatment.RestoreState(state.ToolsOnlyTreatments[1]);
        }

        #endregion 

    }
}