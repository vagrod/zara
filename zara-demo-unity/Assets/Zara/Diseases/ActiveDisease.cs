using System;
using System.Collections.Generic;
using System.Linq;
using ZaraEngine.Diseases.Stages;
using ZaraEngine.Diseases.Stages.Fluent;
using ZaraEngine.Injuries;
using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.Diseases
{
    [Serializable]
    public class ActiveDisease: IAcceptsStateChange
    {

        private readonly IGameController _gc;

        private bool _isDiseaseActivated;
        private bool _isSelfHealActive;
        private bool _isChainInverted;

        private readonly ActiveInjury _linkedInjury;
        
        private DateTime _diseaseStartTime;

        private ChangedVitalsInfo _changedVitals;
        private ChangedVitalsInfo _changedCritialStage;

        public ActiveInjury LinkedInjury => _linkedInjury;

        public ActiveDisease(IGameController gc, ActiveInjury linkedInjury)
        {
            _gc = gc;
            _linkedInjury = linkedInjury;
        }

        public ActiveDisease(IGameController gc, Type diseaseType, DateTime diseaseStartTime) : this(gc, (DiseaseDefinitionBase)Activator.CreateInstance(diseaseType), null, diseaseStartTime)
        {
            
        }

        public ActiveDisease(IGameController gc, Type diseaseType, ActiveInjury linkedInjury,  DateTime diseaseStartTime) : this(gc, (DiseaseDefinitionBase) Activator.CreateInstance(diseaseType), linkedInjury, diseaseStartTime)
        {
           
        }

        public ActiveDisease(IGameController gc, DiseaseDefinitionBase disease, DateTime diseaseStartTime) : this(gc, disease, null, diseaseStartTime)
        {

        }

        public ActiveDisease(IGameController gc, DiseaseDefinitionBase disease, ActiveInjury linkedInjury, DateTime diseaseStartTime)
        {
            _gc = gc;
            _linkedInjury = linkedInjury;
            _diseaseStartTime = diseaseStartTime;

            Disease = disease;

            ComputeDisease();
            Refresh(gc.WorldTime.Value);

            Events.NotifyAll(l => l.DiseaseTriggered(disease, linkedInjury, diseaseStartTime));
        }

        private void ComputeDisease()
        {
            if (Disease.RequiresBodyPart)
            {
                if(_linkedInjury == null)
                    throw new ArgumentException($"The {Disease.Name} disease requires injury to be linked when disease is activated. Provide the linkedInjury argument to ActiveDisease class.");

                Disease.InitializeWithInjury(_linkedInjury.BodyPart);
            }

            var timeOverall = _diseaseStartTime;

            foreach (var stage in Disease.Stages)
            {
                ComputeStageTiming(timeOverall, stage);

                timeOverall = stage.WillEndAt.Value;
            }

            DiseaseStartTime = _diseaseStartTime;
        }

        #region Properties

        public DiseaseStage TreatedStage { get; private set; }

        public bool IsTreated { get; private set; }

        public bool IsActiveNow
        {
            get { return _isDiseaseActivated; }
        }

        public bool IsHealing
        {
            get { return _isSelfHealActive || _isChainInverted; }
        }

        public bool IsSelfHealing
        {
            get { return _isSelfHealActive; }
        }

        public DiseaseDefinitionBase Disease { get; private set; }

        public DateTime DiseaseStartTime { get; private set; }

        #endregion

        #region Finding Active Stage

        public DiseaseStage GetActiveStage(DateTime currentWorldTime)
        {
            var n = 0;

            DiseaseStage foundStage = null;

            foreach (var stage in Disease.Stages)
            {
                if (currentWorldTime >= stage.WillTriggerAt.Value && currentWorldTime <= stage.WillEndAt.Value)
                {
                    foundStage = stage;
                    break;
                }

                n++;
            }

            if (foundStage != null)
            {
                if (!_isDiseaseActivated)
                {
                    _isDiseaseActivated = true;
                    CheckForSelfHeal(foundStage, n);
                }
            }
            else
                _isDiseaseActivated = false;

            return foundStage;
        }

        public void Refresh(DateTime date)
        {
            GetActiveStage(date);
        }

        private void ComputeStageTiming(DateTime startTime, DiseaseStage stage)
        {
            stage.WillTriggerAt = startTime;
            stage.VitalsTargetSeconds = (float)(stage.TargetVitalsTime.HasValue ? stage.TargetVitalsTime.Value.TotalSeconds : stage.StageDuration.TotalSeconds);
            stage.WillEndAt = startTime + stage.StageDuration;
        }

        #endregion

        #region Self-Heal Logic

        private void CheckForSelfHeal(DiseaseStage stage, int stageIndex)
        {
            if (stage.SelfHealChance.WillHappen())
            {
                //(Disease.Name + " will self-heal");

                _isSelfHealActive = true;

                var symtomesToRemove = new List<DiseaseStage>();

                // Let's work with chain copy to avoid strange behavior
                var chainCopy = Disease.Stages.ToList();

                // Remove all stages next to the one that is marked as self-healing
                for (var i = stageIndex + 1; i < chainCopy.Count; i++)
                {
                    symtomesToRemove.Add(chainCopy[i]);
                }

                symtomesToRemove.ForEach(s => chainCopy.Remove(s));
                symtomesToRemove.Clear();

                // Adding heal disease stage
                var healStage = HealthyStageFactory.Get(DiseaseLevels.HealthyStage, Helpers.RollDice(15, 65));

                // Compute timing 
                healStage.WillTriggerAt = stage.WillEndAt;
                healStage.VitalsTargetSeconds = (int)healStage.StageDuration.TotalSeconds;
                healStage.WillEndAt = healStage.WillTriggerAt + healStage.StageDuration;

                // Add healthy stage
                chainCopy.Add(healStage);

                // Swap active disease chains
                Disease.SwapChain(chainCopy);
            }
        }

        #endregion

        #region Inverting Stages Chain

        public void DeclareDiseaseTreated()
        {
            IsTreated = true;
            TreatedStage = null;
        }

        private class ChangedVitalsInfo : IAcceptsStateChange
        {
            public DiseaseLevels Level { get; set; }
            public float? InitialHeartRate { get; set; }
            public float? InitialBloodPressureTop { get; set; }
            public float? InitialBloodPressureBottom { get; set; }
            public float? InitialBodyTemperature { get; set; }
            public TimeSpan InitialStageDuration { get; set; }

            #region State Manage

            public IStateSnippet GetState()
            {
                var state = new ChangedVitalsInfoSnippet
                {
                    Level = this.Level,
                    InitialHeartRate = this.InitialHeartRate,
                    InitialBloodPressureTop = this.InitialBloodPressureTop,
                    InitialBloodPressureBottom = this.InitialBloodPressureBottom,
                    InitialBodyTemperature = this.InitialBodyTemperature,
                    InitialStageDuration = this.InitialStageDuration
                };

                return state;
            }

            public void RestoreState(IStateSnippet savedState)
            {
                var state = (ChangedVitalsInfoSnippet)savedState;

                Level = state.Level;
                InitialHeartRate = state.InitialHeartRate;
                InitialBloodPressureTop = state.InitialBloodPressureTop;
                InitialBloodPressureBottom = state.InitialBloodPressureBottom;
                InitialBodyTemperature = state.InitialBodyTemperature;
                InitialStageDuration = state.InitialStageDuration;
            }

            #endregion

        }

        public void Invert()
        {
            if (_isSelfHealActive)
                return;

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

            // Treatment is started
            TreatedStage = activeStage;

            // Re-calculate disease begin time so that our current disease state will appear unchanged after time inverse
            var startCounting = false;
            var timeOffset = TimeSpan.FromSeconds(0);
            var healthState = _gc.Health.Status;

            // Let's work with chain copy to avoid strange behavior
            var chainCopy = Disease.Stages.ToList();

            // Get time offset that equals the sum of durations of all the stages after the active one
            foreach (var stage in chainCopy)
            {
                if (stage.Level == DiseaseLevels.Critical)
                {
                    _changedCritialStage = new ChangedVitalsInfo
                    {
                        Level = DiseaseLevels.Critical,
                        InitialStageDuration = stage.StageDuration
                    };

                    stage.StageDuration = TimeSpan.FromMinutes(1);
                }
                if (stage.Level == activeStage.Level)
                {
                    _changedVitals = new ChangedVitalsInfo
                    {
                        Level = stage.Level,
                        InitialStageDuration = stage.StageDuration
                    };

                    if (stage.Level != DiseaseLevels.Critical)
                    {
                        stage.StageDuration = TimeSpan.FromMinutes(Helpers.RollDice(7, 14));
                    }
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

            // New disease start time is a current time minus stages offset
            var timeOverall = _gc.WorldTime.Value.AddMinutes(-1) - timeOffset;

            DiseaseStartTime = timeOverall;

            // Reverse the chain
            chainCopy.Reverse();

            // Add healthy stage
            chainCopy.Add(HealthyStageFactory.Get(DiseaseLevels.HealthyStage, (int)Disease.Stages.First().StageDuration.TotalHours));

            // Recompute timing for all stages
            foreach (var stage in chainCopy)
            {
                ComputeStageTiming(timeOverall, stage);

                if (stage.Level == activeStage.Level)
                {
                    _changedVitals.InitialBloodPressureBottom = stage.TargetBloodPressureBottom;
                    _changedVitals.InitialBloodPressureTop = stage.TargetBloodPressureTop;
                    _changedVitals.InitialBodyTemperature = stage.TargetBodyTemperature;
                    _changedVitals.InitialHeartRate = stage.TargetHeartRate;

                    stage.TargetBloodPressureTop = healthState.BloodPressureTop;
                    stage.TargetBloodPressureBottom = healthState.BloodPressureBottom;
                    stage.TargetBodyTemperature = healthState.BodyTemperature;
                    stage.TargetHeartRate = healthState.HeartRate;
                }

                timeOverall = stage.WillEndAt.Value;
            }

            // Swap active disease chains
            Disease.SwapChain(chainCopy);
        }

        public void InvertBack()
        {
            if (_isSelfHealActive)
                return;

            if (!_isChainInverted)
                return;

            _isChainInverted = false;

            // Treatment failed
            TreatedStage = null;

            // Get the active stage
            var activeStage = GetActiveStage(_gc.WorldTime.Value);

            // Do not revert healthy stage
            if (activeStage == null || activeStage.Level == DiseaseLevels.HealthyStage)
            {
                _isChainInverted = false;
                return;
            }

            Disease.OnResumeDisease();

            // Re-calculate disease begin time so that our current disease state will appear unchanged after time inverse
            var startCounting = false;
            var timeOffset = TimeSpan.FromSeconds(0);

            // Let's work with chain copy to avoid strange behavior
            var chainCopy = Disease.Stages.ToList();

            // Remove healing stage
            chainCopy.Remove(chainCopy.Last());

            // Get time offset that equals the sum of durations of all the stages after the active one
            foreach (var stage in chainCopy)
            {
                if (startCounting)
                {
                    timeOffset += stage.StageDuration;
                }

                if (stage == activeStage)
                {
                    startCounting = true;
                }
            }

            // New disease start time is a current time minus stages offset
            var timeOverall = _gc.WorldTime.Value - timeOffset;

            DiseaseStartTime = timeOverall;

            // Reverse the chain
            chainCopy.Reverse();

            // Recompute timing for all stages
            foreach (var stage in chainCopy)
            {
                if (stage.Level == DiseaseLevels.Critical)
                {
                    if (_changedCritialStage != null)
                    {
                        stage.StageDuration = _changedCritialStage.InitialStageDuration;
                    }
                }

                ComputeStageTiming(timeOverall, stage);

                if (_changedVitals != null)
                {
                    if (_changedVitals.Level == stage.Level)
                    {
                        stage.TargetBloodPressureTop = _changedVitals.InitialBloodPressureTop;
                        stage.TargetBloodPressureBottom = _changedVitals.InitialBloodPressureBottom;
                        stage.TargetBodyTemperature = _changedVitals.InitialBodyTemperature;
                        stage.TargetHeartRate = _changedVitals.InitialHeartRate;
                        stage.StageDuration = _changedVitals.InitialStageDuration;
                    }
                }

                timeOverall = stage.WillEndAt.Value;
            }

            // Swap active disease chains
            Disease.SwapChain(chainCopy);

            _changedVitals = null;
            _changedCritialStage = null;

            Events.NotifyAll(l => l.DiseaseReActivated(Disease));
        }

        #endregion

        #region Applying Inventory Items

        public void OnConsumeItem(IGameController gc, InventoryConsumableItemBase consumable)
        {
            if (IsTreated)
                return;

            if (TreatedStage != null)
            {
                TreatedStage.OnConsumeItem(gc, consumable, this);
            }
            else
            {
                var stage = GetActiveStage(gc.WorldTime.Value);

                if (stage != null)
                    stage.OnConsumeItem(gc, consumable, this);
            }
        }

        public void OnApplianceTaken(IGameController gc, InventoryMedicalItemBase appliance, BodyParts bodyPart)
        {
            if (IsTreated)
                return;

            if (TreatedStage != null)
            {
                if(TreatedStage.OnApplianceTaken != null)
                    TreatedStage.OnApplianceTaken(gc, new ApplianceInfo { Appliance = appliance, BodyPart = bodyPart }, this);
            }
            else
            {
                var stage = GetActiveStage(gc.WorldTime.Value);

                if (stage != null && stage.OnApplianceTaken != null)
                    stage.OnApplianceTaken(gc, new ApplianceInfo { Appliance = appliance, BodyPart = bodyPart }, this);
            }
        }

        public bool CanApplyItem(IGameController gc, string itemName)
        {
            if (IsTreated)
                return false;

            var stage = GetActiveStage(gc.WorldTime.Value);

            if (stage != null && stage.AcceptedTreatmentItems != null)
            {
                return stage.AcceptedTreatmentItems.Contains(itemName);
            }

            return false;
        }

        #endregion

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new ActiveDiseaseSnippet
            {
                DiseaseId = Disease.Id,
                InjuryId = _linkedInjury?.Injury?.Id,
                DiseaseType = Disease.GetType(),
                IsDiseaseActivated = _isDiseaseActivated,
                IsSelfHealActive = _isSelfHealActive,
                IsChainInverted = _isChainInverted,
                DiseaseStartTime = _diseaseStartTime,
                TreatedStageLevel = TreatedStage?.Level,
                IsTreated = this.IsTreated,
            };

            state.ChildStates.Add("ChangedVitals", _changedVitals?.GetState());
            state.ChildStates.Add("ChangedCritialStage", _changedCritialStage?.GetState());
            state.ChildStates.Add("Treatments", Disease.GetState());

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (ActiveDiseaseSnippet)savedState;

            Disease = (DiseaseDefinitionBase)Activator.CreateInstance(state.DiseaseType);
            
            _isDiseaseActivated = state.IsDiseaseActivated;
            _isSelfHealActive = state.IsSelfHealActive;
            _diseaseStartTime = state.DiseaseStartTime;

            IsTreated = state.IsTreated;

            ComputeDisease();

            if (state.TreatedStageLevel.HasValue)
                Invert();
            else
                TreatedStage = null;

            _isChainInverted = state.IsChainInverted;
            
            if (state.ChildStates["ChangedVitals"] != null)
            {
                var o = (ChangedVitalsInfoSnippet)state.ChildStates["ChangedVitals"];

                if (!o.IsEmpty)
                {
                    if (_changedVitals == null)
                        _changedVitals = new ChangedVitalsInfo();

                    _changedVitals.RestoreState(state.ChildStates["ChangedVitals"]);
                }
            }

            if (state.ChildStates["ChangedCritialStage"] != null)
            {
                var o = (ChangedVitalsInfoSnippet)state.ChildStates["ChangedCritialStage"];

                if (!o.IsEmpty)
                {
                    if (_changedCritialStage == null)
                        _changedCritialStage = new ChangedVitalsInfo();

                    _changedCritialStage.RestoreState(state.ChildStates["ChangedCritialStage"]);
                }
            }

            Disease.RestoreState(state.ChildStates["Treatments"]);

            Refresh(_gc.WorldTime.Value);
        }

        #endregion 

    }
}
