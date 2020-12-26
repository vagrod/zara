using System;
using ZaraEngine.Diseases;
using ZaraEngine.Inventory;

namespace ZaraEngine.Injuries.Stages
{
    public class InjuryStage
    {

        public InjuryStage()
        {
            CanRun = true;
        }

        public DiseaseLevels Level { get; set; }

        public DiseaseDefinitionBase TriggeringDisease { get; set; }

        public TimeSpan? SelfHealTime { get; set; }

        public TimeSpan StageDuration { get; set; }

        public bool IsFracture
        {
            get { return IsOpenFracture || IsClosedFracture; }
        }

        public bool IsOpenFracture { get; set; }

        public bool IsClosedFracture { get; set; }

        public bool IsCut { get; set; }

        public float? BloodDrainPerSecond { get; set; }

        public float? StaminaDrainPerSecond { get; set; }

        public float? FatigueIncreasePerSecond { get; set; }

        public bool CanRun { get; set; }

        public float WalkSpeedDecrease { get; set; }

        public DateTime? WillTriggerAt { get; set; }

        public DateTime? WillEndAt { get; set; }

        public DateTime? SelfHealAt { get; set; }

        public bool IsDiseaseTriggered { get; set; }

        public string Description { get; set; }

        public int DiseaseProbability { get; set; }

        internal Func<IGameController, InventoryMedicalItemBase, Player.BodyParts, ActiveInjury, bool> OnApplySpecialItem { get; set; }
    }
}
