using System;
using System.Linq;
using ZaraEngine.Diseases;
using ZaraEngine.Injuries.Stages;
using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Injuries
{

    public enum BodyParts
    {
        Unknown = -1,
        Forehead = 0,
        Nape = 1,
        Eye = 2,
        Ear = 3,
        Nose = 4,
        Throat = 5,
        LeftShoulder = 6,
        RightShoulder = 7,
        LeftForearm = 8,
        RightForearm = 9,
        LeftSpokebone = 10,
        RightSpokebone = 11,
        LeftBrush = 12,
        RightBrush = 13,
        LeftChest = 14,
        RightChest = 15,
        Belly = 16,
        LeftHip = 17,
        RightHip = 18,
        LeftKnee = 19,
        RightKnee = 20,
        LeftShin = 21,
        RightShin = 22,
        LeftFoot = 23,
        RightFoot = 24,
        Back = 25
    }

    [Serializable]
    public class ActiveInjury : IAcceptsStateChange
    {

        private bool _isInjuryActivated;
        private bool _isChainInverted;

        private readonly IGameController _gc;

        public ActiveInjury(IGameController gc)
        {
            _gc = gc;
        }

        public ActiveInjury(IGameController gc, Type injuryType, BodyParts bodyPart, DateTime injuryTriggerTime)
        {
            _gc = gc;

            Injury = (InjuryBase)Activator.CreateInstance(injuryType);
            BodyPart = bodyPart;
            InjuryTriggerTime = injuryTriggerTime;

            SetUpActiveStage(injuryTriggerTime);
        }

        private void SetUpActiveStage(DateTime injuryTriggerTime)
        {
            var timeOverall = injuryTriggerTime;

            foreach (var stage in Injury.Stages)
            {
                stage.WillTriggerAt = timeOverall;

                if (stage.SelfHealTime.HasValue)
                    stage.SelfHealAt = timeOverall + stage.SelfHealTime.Value;

                stage.WillEndAt = timeOverall + stage.StageDuration;

                timeOverall = stage.WillEndAt.Value;
            }

            GetActiveStage(_gc.WorldTime.Value);
        }

        public bool IsActiveNow
        {
            get { return _isInjuryActivated; }
        }

        public BodyParts BodyPart { get; set; }

        public InjuryBase Injury { get; set; }

        public DateTime InjuryTriggerTime { get; set; }

        public InjuryStage TreatedStage { get; private set; }

        public bool IsTreated { get; private set; }
        public bool IsDiseaseProbabilityChecked { get; set; }


        public InjuryStage GetActiveStage(DateTime currentWorldTime)
        {
            InjuryStage foundStage = null;

            foreach (var stage in Injury.Stages)
            {
                if (currentWorldTime >= stage.WillTriggerAt.Value && currentWorldTime <= stage.WillEndAt.Value)
                {
                    foundStage = stage;

                    break;
                }
            }

            if (foundStage != null)
            {
                if (!_isInjuryActivated)
                {
                    // Let's check for body appliances for this body part. We need to remove all bandages and splints when this body part receives a new injury
                    var bodyAppliances = _gc.Body.Appliances.Where(x => x.BodyPart == BodyPart).ToList();

                    foreach (var appliance in bodyAppliances)
                    {
                        _gc.Body.Appliances.Remove(appliance);
                    }

                    _isInjuryActivated = true;
                }

                if (foundStage.SelfHealAt.HasValue && currentWorldTime >= foundStage.SelfHealAt.Value)
                {
                    _isInjuryActivated = false;

                    DeclareInjuryTreated();

                    return null;
                }
            }
            else
                _isInjuryActivated = false;

            return foundStage;
        }

        public void OnApplianceTaken(IGameController gc, InventoryMedicalItemBase item, BodyParts bodyPart)
        {
            InjuryStage stage = null;

            if (TreatedStage == null)
                stage = GetActiveStage(_gc.WorldTime.Value);
            else
                stage = TreatedStage;

            if (stage != null && stage.OnApplySpecialItem != null)
                stage.OnApplySpecialItem(gc, item, bodyPart, this);
        }

        #region Inverting Stages Chain

        public void DeclareInjuryTreated()
        {
            _isInjuryActivated = false;

            IsTreated = true;
            TreatedStage = null;
        }

        public void Invert()
        {
            if (_isChainInverted)
                return;

            _isChainInverted = true;

            // Get the active stage
            var activeStage = GetActiveStage(_gc.WorldTime.Value);

            if (activeStage == null)
            {
                _isChainInverted = false;
                return;
            }

            if (activeStage.SelfHealAt.HasValue)
                return;

            // Treatment is started
            TreatedStage = activeStage;

            // Re-calculate injury begin time so that our current injury state will appear unchanged after time inverse
            var startCounting = false;
            var timeOffset = TimeSpan.FromSeconds(0);

            // Let's work with chain copy to avoid strange behavior
            var chainCopy = Injury.Stages.ToList();

            // Get time offset that equals the sum of durations of all the stages after the active one
            foreach (var stage in chainCopy)
            {
                if (stage.Level == DiseaseLevels.Critical)
                {
                    stage.StageDuration = TimeSpan.FromMinutes(1);
                }
                else if (stage.Level == activeStage.Level)
                {
                    if (stage.Level != DiseaseLevels.Critical)
                    {
                        stage.StageDuration = TimeSpan.FromMinutes(Helpers.RollDice(7, 14));
                    }
                }
                else
                {
                    stage.StageDuration = TimeSpan.FromMinutes(stage.StageDuration.TotalMinutes / 2);
                }

                if (startCounting)
                {
                    timeOffset += stage.StageDuration;
                }

                if (stage == activeStage)
                {
                    startCounting = true;
                }
            }

            // New injury start time is a current time minus stages offset
            var timeOverall = _gc.WorldTime.Value - timeOffset;

            InjuryTriggerTime = timeOverall;

            // Reverse the chain
            chainCopy.Reverse();

            // Recompute timing for all stages
            foreach (var stage in chainCopy)
            {
                ComputeStageTiming(timeOverall, stage);

                timeOverall = stage.WillEndAt.Value;
            }

            // Swap active injury chains
            Injury.SwapChain(chainCopy);
        }

        private void ComputeStageTiming(DateTime startTime, InjuryStage stage)
        {
            stage.WillTriggerAt = startTime;
            stage.WillEndAt = startTime + stage.StageDuration;
        }

        #endregion

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new ActiveInjurySnippet
            {
                BodyPart = this.BodyPart,
                InjuryId = Injury.Id,
                InjuryTriggerTime = this.InjuryTriggerTime,
                InjuryType = Injury.GetType(),
                IsChainInverted = _isChainInverted,
                IsDiseaseProbabilityChecked = this.IsDiseaseProbabilityChecked,
                IsInjuryActivated = _isInjuryActivated,
                IsTreated = this.IsTreated,
                TreatedStageLevel = TreatedStage?.Level
            };

            state.ChildStates.Add("Treatments", Injury.GetState());

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (ActiveInjurySnippet)savedState;

            Injury = (InjuryBase)Activator.CreateInstance(state.InjuryType);

            SetUpActiveStage(state.InjuryTriggerTime);

            Injury.RestoreState(state.ChildStates["Treatments"]);

            if (state.TreatedStageLevel.HasValue)
                TreatedStage = Injury.Stages.FirstOrDefault(x => x.Level == state.TreatedStageLevel.Value);
            else
                TreatedStage = null;

            _isChainInverted = state.IsChainInverted;
            _isInjuryActivated = state.IsInjuryActivated;

            InjuryTriggerTime = state.InjuryTriggerTime;
            IsDiseaseProbabilityChecked = state.IsDiseaseProbabilityChecked;
            BodyPart = state.BodyPart;
            IsTreated = state.IsTreated;
        }

        #endregion 

    }
}