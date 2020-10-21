using System;
using ZaraEngine.Diseases;
using ZaraEngine.Inventory;

namespace ZaraEngine.Injuries.Stages.Fluent
{
    public class InjuryStageBuilder : IInjuryStageNodeStart, IInjuryStageEnd, IInjuryStageControls, IInjuryStageTreatmentNode, IInjuryStageTreatmentSpecialItem, IInjuryStageDrains, IInjuryDescription, IInjuryStageDrainsNode, IInjuryStageNodeHealthImpact, IInjuryStageNodeType, IInjuryStageNodeTiming
    {

        private readonly InjuryStage _obj;

        public InjuryStageBuilder()
        {
            _obj = new InjuryStage();
        }

        public static IInjuryStageNodeStart NewStage()
        {
            return new InjuryStageBuilder();
        }

        public IInjuryDescription WithLevelOfSeriousness(DiseaseLevels level)
        {
            _obj.Level = level;

            return this;
        }

        public InjuryStage Build()
        {
            return _obj;
        }

        public IInjuryStageControls WillNotBeAbleToRun()
        {
            _obj.CanRun = false;

            return this;
        }

        public IInjuryStageTreatmentNode DescreasesMoveSpeed(float amount)
        {
            _obj.WalkSpeedDecrease = amount;

            return this;
        }

        public IInjuryStageTreatmentNode NoSpeedImpact()
        {
            return this;
        }

        public IInjuryStageDrains Drains
        {
            get { return this; }
        }

        public IInjuryStageControls NoDrains()
        {
            return this;
        }

        public IInjuryStageDrainsNode TriggersDisease<T>(int probability) where T : DiseaseDefinitionBase
        {
            _obj.DiseaseProbability = probability;
            _obj.TriggeringDisease = Activator.CreateInstance<T>();

            return this;
        }

        public IInjuryStageDrainsNode WillSelfHealInHours(int value)
        {
            _obj.SelfHealTime = TimeSpan.FromHours(value);

            return this;
        }

        public IInjuryStageDrainsNode NoSelfHeal()
        {
            return this;
        }

        public IInjuryStageNodeTiming OpenFracture()
        {
            _obj.IsOpenFracture = true;

            return this;
        }

        public IInjuryStageNodeTiming ClosedFracture()
        {
            _obj.IsClosedFracture = true;

            return this;
        }

        public IInjuryStageNodeTiming Cut()
        {
            _obj.IsCut = true;

            return this;
        }

        public IInjuryStageNodeTiming BasicInjury()
        {
            return this;
        }

        public IInjuryStageDrains BloodPerSecond(float value)
        {
            _obj.BloodDrainPerSecond = value;

            return this;
        }

        public IInjuryStageDrains StaminaPerSecond(float value)
        {
            _obj.StaminaDrainPerSecond = value;

            return this;
        }

        public IInjuryStageDrains FatiguePerSecond(float value)
        {
            _obj.FatigueIncreasePerSecond = value;

            return this;
        }

        public IInjuryStageControls WillAffectControls()
        {
            return this;
        }

        public IInjuryStageTreatmentNode WillNotAffectControls()
        {
            return this;
        }

        public IInjuryStageNodeHealthImpact WillLastForHours(int value)
        {
            _obj.StageDuration = TimeSpan.FromHours(value);

            return this;
        }

        public IInjuryStageNodeHealthImpact WillLastForever()
        {
            _obj.StageDuration = TimeSpan.FromHours(10000);

            return this;
        }

        public IInjuryStageNodeType WithDescription(string description)
        {
            _obj.Description = description;

            return this;
        }

        public IInjuryStageNodeType NoDescription()
        {
            return this;
        }

        public IInjuryStageTreatmentSpecialItem Treatment
        {
            get { return this; }
        }

        public IInjuryStageEnd NoTreatment()
        {
            return this;
        }


        public IInjuryStageEnd WithTreatmentAction(Func<IGameController, InventoryMedicalItemBase, BodyParts, ActiveInjury, bool> treatmentAction)
        {
            _obj.OnApplySpecialItem = treatmentAction;

            return this;
        }
    }
}
