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
    public class DeepCut : InjuryBase
    {

        private readonly ToolsOnlyInjuryTreatment _initialStageTreatment;
        private readonly ToolsOnlyInjuryTreatment _progressingStageTreatment;
        private readonly ToolsOnlyInjuryTreatment _worryingStageTreatment;

        public DeepCut()
        {
            _initialStageTreatment = new ToolsOnlyInjuryTreatment(MedicalAppliancesGroup.AntisepticGroup, InventoryController.MedicalItems.Bandage);
            _progressingStageTreatment = new ToolsOnlyInjuryTreatment(MedicalAppliancesGroup.AntisepticGroup, InventoryController.MedicalItems.Bandage);
            _worryingStageTreatment = new ToolsOnlyInjuryTreatment(InventoryController.MedicalItems.SuctionPump, MedicalAppliancesGroup.AntisepticGroup, InventoryController.MedicalItems.Bandage);

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
                        .WithTreatmentAction(_initialStageTreatment.OnApplianceTaken)
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
                        .WithTreatmentAction(_progressingStageTreatment.OnApplianceTaken)
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
