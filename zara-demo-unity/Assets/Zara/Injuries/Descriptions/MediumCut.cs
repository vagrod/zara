using ZaraEngine.Diseases;
using ZaraEngine.Injuries.Stages.Fluent;
using ZaraEngine.Injuries.Treatment;
using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Injuries
{
    public class MediumCut : InjuryBase
    {

        private readonly ToolsOnlyInjuryTreatment _initialStageTreatment;
        private readonly ToolsOnlyInjuryTreatment _progressingStageTreatment;
        private readonly ToolsOnlyInjuryTreatment _worryingStageTreatment;

        public MediumCut()
        {
            _initialStageTreatment = new ToolsOnlyInjuryTreatment(MedicalAppliancesGroup.AntisepticGroup, InventoryController.MedicalItems.Bandage);
            _progressingStageTreatment = new ToolsOnlyInjuryTreatment(MedicalAppliancesGroup.AntisepticGroup, InventoryController.MedicalItems.Bandage);
            _worryingStageTreatment = new ToolsOnlyInjuryTreatment(InventoryController.MedicalItems.SuctionPump, MedicalAppliancesGroup.AntisepticGroup, InventoryController.MedicalItems.Bandage);

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
                        .WithTreatmentAction(_initialStageTreatment.OnApplianceTaken)
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
                        .WithTreatmentAction(_progressingStageTreatment.OnApplianceTaken)
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
                        .WithTreatmentAction(_worryingStageTreatment.OnApplianceTaken)
                    .Build()
            };
        }

        #region State Manage

        public override IStateSnippet GetState()
        {
            var state = new InjuryTreatmentSnippet();

            state.ToolsOnlyTreatments.Add((ToolsOnlyInjuryTreatmentSnippet)_initialStageTreatment.GetState());
            state.ToolsOnlyTreatments.Add((ToolsOnlyInjuryTreatmentSnippet)_progressingStageTreatment.GetState());
            state.ToolsOnlyTreatments.Add((ToolsOnlyInjuryTreatmentSnippet)_worryingStageTreatment.GetState());

            return state;
        }

        public override void RestoreState(IStateSnippet savedState)
        {
            var state = (InjuryTreatmentSnippet)savedState;

            _initialStageTreatment.RestoreState(state.ToolsOnlyTreatments[0]);
            _progressingStageTreatment.RestoreState(state.ToolsOnlyTreatments[1]);
            _worryingStageTreatment.RestoreState(state.ToolsOnlyTreatments[2]);
        }

        #endregion 

    }
}
