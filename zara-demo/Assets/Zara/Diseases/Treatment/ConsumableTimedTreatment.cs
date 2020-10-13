using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaraEngine.Diseases.Stages;
using ZaraEngine.Inventory;

namespace ZaraEngine.Diseases.Treatment
{
    public class ConsumableTimedTreatment
    {

        private const int TimingDeltaBetweenAllowedConsuming = 20; // Game minutes

        public Action OnTreatmentStarted { get; set; }
        public Action OnTreatmentEnded { get; set; }
        public Action OnTreatmentFailed { get; set; }

        public bool IsNodePart { get; set; }

        private readonly MedicalConsumablesGroup _consumables;
        private readonly int _timingInGameMinutes;
        private readonly int _countToConsume;

        private DiseaseLevels _treatedLevel;

        public bool IsFailed { get; set; }
        public bool IsStarted{ get; set; }

        private readonly List<DateTime> _consumedTimes = new List<DateTime>();

        private int _inTimeConsumedCount;

        public bool IsFinished { get; private set; }

        public ConsumableTimedTreatment(MedicalConsumablesGroup consumables) : this(consumables, 0, 1)
        {

        }
        public ConsumableTimedTreatment(MedicalConsumablesGroup consumables, int timingInGameMinutes, int count)
        {
            _consumables = consumables;
            _timingInGameMinutes = timingInGameMinutes;
            _countToConsume = count;
        }

        public ConsumableTimedTreatment(DiseaseLevels treatedLevel, MedicalConsumablesGroup consumables) : this(treatedLevel, consumables, 0, 1)
        {

        }
        public ConsumableTimedTreatment(DiseaseLevels treatedLevel, MedicalConsumablesGroup consumables, int timingInGameMinutes, int count)
        {
            _treatedLevel = treatedLevel;
            _consumables = consumables;
            _timingInGameMinutes = timingInGameMinutes;
            _countToConsume = count;
        }

        internal void SetTreatedLevel(DiseaseLevels level)
        {
            _treatedLevel = level;
        }

        public bool OnItemConsumed(IGameController gc, InventoryConsumableItemBase consumable, ActiveDisease disease)
        {
            if (IsFinished)
                return false;

            if (!gc.WorldTime.HasValue)
                return false;

            if (disease.IsSelfHealing)
                return false;

            var treatedStage = disease.TreatedStage;

            if (treatedStage == null)
                treatedStage = disease.GetActiveStage(gc.WorldTime.Value);

            if (treatedStage == null)
                return false;

            var isTreatedLevel = treatedStage.Level == _treatedLevel;

            if (_consumables.Items.Contains(consumable.Name))
            {
                var currentTime = gc.WorldTime.Value;

                if (_consumedTimes.Count == 0)
                {
                    // First consume
                    _inTimeConsumedCount++;
                    _consumedTimes.Add(currentTime);

                    if (isTreatedLevel)
                    {
                        IsFinished = false;

                        CheckIfTreatmentFinished(disease);

                        if (OnTreatmentStarted != null)
                            OnTreatmentStarted.Invoke();

                        //("Disease consumables treatment started.");

                        IsStarted = true;

                        if (!IsNodePart && !IsFinished)
                        {
                            //("Overall disease treatment started.");

                            Events.NotifyAll(l => l.DiseaseTreatmentStarted(disease.Disease));

                            // We're starting to heal
                            disease.Invert();
                        }
                    }
                }
                else
                {
                    IsFinished = false;

                    var lastTime = _consumedTimes.Last();
                    var minutes = (currentTime - lastTime).TotalMinutes;

                    if (minutes <= _timingInGameMinutes + TimingDeltaBetweenAllowedConsuming && minutes >=_timingInGameMinutes - TimingDeltaBetweenAllowedConsuming)
                    {
                        _inTimeConsumedCount++;

                        _consumedTimes.Add(currentTime);

                        if (isTreatedLevel)
                            CheckIfTreatmentFinished(disease);
                    }
                }

                return true;
            }

            return false;
        }

        private void CheckIfTreatmentFinished(ActiveDisease disease)
        {
            if (_inTimeConsumedCount == _countToConsume)
            {
                IsFinished = true;

                if (OnTreatmentEnded != null)
                    OnTreatmentEnded.Invoke();

                if (!IsNodePart)
                {
                    disease.DeclareDiseaseTreated();

                    disease.Invert();

                    Events.NotifyAll(l => l.DiseaseHealed(disease.Disease));
                }

                //("Disease treatment finished.");
            }
            else
            {
                //("Disease treatment continued. Healing.");

                if (!IsNodePart && IsStarted)
                {
                    Events.NotifyAll(l => l.DiseaseHealingContinued(disease.Disease));
                }
            }
        }

        public void Check(ActiveDisease disease, IGameController gc)
        {
            if(_consumedTimes.Count == 0)
                return;

            if (!disease.IsHealing)
                return;

            if (disease.IsSelfHealing)
                return;

            if (!gc.WorldTime.HasValue)
                return;

            if (disease.TreatedStage == null)
                return;

            var activeStage = disease.GetActiveStage(gc.WorldTime.Value);

            if (activeStage == null || activeStage.Level == DiseaseLevels.HealthyStage)
                return;

            var currentTime = gc.WorldTime.Value;
            var lastConsumedTime = _consumedTimes.Last();
            var minutes = (currentTime - lastConsumedTime).TotalMinutes;

            if (minutes > _timingInGameMinutes + TimingDeltaBetweenAllowedConsuming)
            {
                //("You missed your treatment. You sick again.");

                if (!IsNodePart)
                {
                    Events.NotifyAll(l => l.DiseaseStartProgressing(disease.Disease));
                }

                Reset();

                IsFinished = false;
                IsFailed = true;

                if (OnTreatmentFailed != null)
                    OnTreatmentFailed.Invoke();

                disease.InvertBack();
            }
        }

        public void Reset()
        {
            IsFinished = false;
            IsStarted = false;

            _consumedTimes.Clear();
            _inTimeConsumedCount = 0;
        }

    }
}
