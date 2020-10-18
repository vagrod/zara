using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases.Stages;
using ZaraEngine.Diseases.Stages.Fluent;
using ZaraEngine.Diseases.Treatment;
using ZaraEngine.Injuries;
using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Diseases
{
    public class BloodPoisoning : DiseaseDefinitionBase
    {
        private ApplianceTimedTreatmentNode _progressingStageTreatment;
        private ApplianceTimedTreatmentNode _worryingStageTreatment;
        private ApplianceTimedTreatmentNode _criticalingStageTreatment;

        public BodyParts BodyPart { get; private set; }

        public BloodPoisoning()
        {
            Name = "BloodPoisoning";
            RequiresBodyPart = true;
        }

        public override void InitializeWithInjury(BodyParts initialInjury)
        {
            BodyPart = initialInjury;

            // Describe treatment for each stage
            _progressingStageTreatment = new ApplianceTimedTreatmentNode(DiseaseLevels.Progressing,
                new ApplianceTimedTreatment(initialInjury, InventoryController.MedicalItems.AntibioticEmbrocation),
                new ApplianceTimedTreatment(null, InventoryController.MedicalItems.DoripenemSyringe)
            );
            _worryingStageTreatment = new ApplianceTimedTreatmentNode(DiseaseLevels.Worrying,
                new ApplianceTimedTreatment(initialInjury, InventoryController.MedicalItems.AntibioticEmbrocation),
                new ApplianceTimedTreatment(null, InventoryController.MedicalItems.DoripenemSyringe, 90, 2)
            );
            _criticalingStageTreatment = new ApplianceTimedTreatmentNode(DiseaseLevels.Critical,
                new ApplianceTimedTreatment(initialInjury, InventoryController.MedicalItems.AntibioticEmbrocation),
                new ApplianceTimedTreatment(null, InventoryController.MedicalItems.DoripenemSyringe, 90, 3)
            );

            Stages = new List<DiseaseStage>(new[]
             {
                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Progressing)
                                             .NoSelfHeal()
                                             .Vitals
                                                 .WithTargetHeartRate(95)
                                                 .WithTargetBloodPressure(154, 83)
                                                 .WithTargetBodyTemperature(39.6f)
                                             .WillReachTargetsInHours(1)
                                             .AndLastForHours(3)
                                             .AdditionalEffects
                                                 .WithLowChanceOfBlackouts()
                                                 .WithLowAdditionalStaminaDrain()
                                             .Disorders
                                             .NotDeadly()
                                             .Drain
                                                 .FatigueIncreasePerSecond(0.0004f)
                                             .Treatment
                                                 .WithAppliance(_progressingStageTreatment.OnApplianceTaken)
                                                 .AndWithoutSpecialItems()
                                             .Build(),

                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Worrying)
                                             .NoSelfHeal()
                                             .Vitals
                                                 .WithTargetHeartRate(126)
                                                 .WithTargetBloodPressure(196, 98)
                                                 .WithTargetBodyTemperature(40.1f)
                                             .WillReachTargetsInHours(2)
                                             .AndLastForHours(3).AndMinutes(10)
                                             .AdditionalEffects
                                                 .WithMediumChanceOfBlackouts()
                                                 .WithMediumAdditionalStaminaDrain()
                                             .NoDisorders()
                                             .Drain
                                                 .WaterPerSecond(0.01f)
                                                 .FatigueIncreasePerSecond(0.0006f)
                                             .Treatment
                                                 .WithAppliance(_worryingStageTreatment.OnApplianceTaken)
                                                 .AndWithoutSpecialItems()
                                             .Build(),

                StageBuilder.NewStage().WithLevelOfSeriousness(DiseaseLevels.Critical)
                                             .NoSelfHeal()
                                             .Vitals
                                                 .WithTargetHeartRate(148)
                                                 .WithTargetBloodPressure(204, 112)
                                                 .WithTargetBodyTemperature(41.6f)
                                             .WillReachTargetsInHours(2)
                                             .AndLastUntilEnd()
                                             .AdditionalEffects
                                                 .WithMediumChanceOfDizziness()
                                                 .WithLowChanceOfBlackouts()
                                                 .WithHighAdditionalStaminaDrain()
                                             .Disorders
                                                 .WillNotBeAbleToRun()
                                                 .NotDeadly()
                                             .Drain
                                                 .WaterPerSecond(0.015f)
                                                 .FoodPerSecond(0.008f)
                                                 .FatigueIncreasePerSecond(0.00085f)
                                             .Treatment
                                                 .WithAppliance(_criticalingStageTreatment.OnApplianceTaken)
                                                 .AndWithoutSpecialItems()
                                             .Build()
            });
        }

        public override void OnResumeDisease()
        {
            _progressingStageTreatment.Reset();
            _worryingStageTreatment.Reset();
            _criticalingStageTreatment.Reset();
        }

        public override void Check(ActiveDisease disease, IGameController gc)
        {
            _progressingStageTreatment.Check(disease, gc);
            _worryingStageTreatment.Check(disease, gc);
            _criticalingStageTreatment.Check(disease, gc);
        }

        #region State Manage

        public override IStateSnippet GetState()
        {
            var state = new DiseaseTreatmentSnippet();

            state.ApplianceTimedTreatmentNodes.Add((ApplianceTimedTreatmentNodeSnippet)_progressingStageTreatment.GetState());
            state.ApplianceTimedTreatmentNodes.Add((ApplianceTimedTreatmentNodeSnippet)_worryingStageTreatment.GetState());
            state.ApplianceTimedTreatmentNodes.Add((ApplianceTimedTreatmentNodeSnippet)_criticalingStageTreatment.GetState());

            return state;
        }

        public override void RestoreState(IStateSnippet savedState)
        {
            var state = (DiseaseTreatmentSnippet)savedState;

            _progressingStageTreatment.RestoreState(state.ApplianceTimedTreatmentNodes[0]);
            _worryingStageTreatment.RestoreState(state.ApplianceTimedTreatmentNodes[1]);
            _criticalingStageTreatment.RestoreState(state.ApplianceTimedTreatmentNodes[2]);
        }

        #endregion 

    }
}
