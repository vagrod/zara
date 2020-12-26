using System;
using System.Collections.Generic;
using System.Linq;
using ZaraEngine.Injuries;
using ZaraEngine.Inventory;
using ZaraEngine.StateManaging;

namespace ZaraEngine.HealthEngine
{
    public class ActiveMedicalAgent : IActiveMedicalAgent, IAcceptsStateChange
    {

        private readonly IGameController _gc;
        private readonly MedicalConsumablesGroup _group;
        private readonly CurveTypes _activationType;

        private MultiKeyedLerp _activationCurve;

        private float _gameMinutesAgentIsActive;
        private List<DateTime> _timesTaken = new List<DateTime>();

        public List<DateTime> DosesTaken
        {
            get { return _timesTaken.ToList(); }
        }

        public enum CurveTypes
        {
            ActiveImmediately,
            SlowActivation,
            ActiveInSecondHalf
        }

        public ActiveMedicalAgent(IGameController gc, MedicalConsumablesGroup group, CurveTypes activationType, float gameMinutesAgentIsActive)
        {
            _activationType = activationType;
            _gameMinutesAgentIsActive = gameMinutesAgentIsActive;
            _gc = gc;
            _group = group;

            BuildCurve();
        }

        public MedicalConsumablesGroup MedicalGroup
        {
            get { return _group; }
        }

        public DateTime? LastTaken
        {
            get
            {
                return _timesTaken.LastOrDefault();
            }
        }

        public int ActiveDosesCount
        {
            get { return _timesTaken.Count; }
        }

        public bool IsActive
        {
            get { return _timesTaken.Any(); }
        }

        public float PercentOfActivity
        {
            get
            {
                return _activationCurve.Evaluate(PercentOfPresence);
            }
        }

        public float PercentOfPresence
        {
            get
            {
                if (!_timesTaken.Any())
                    return 0;

                var firstActive = _timesTaken.First();

                var minutesSinceTaken = (_gc.WorldTime.Value - firstActive).TotalMinutes;
                var percent = (float)minutesSinceTaken / _gameMinutesAgentIsActive;

                percent *= 100f;

                if (percent > 100f)
                    percent = 100f;

                if (percent < 0f)
                    percent = 0f;

                return percent;
            }
        }

        public void Check()
        {
            if (!_timesTaken.Any())
                return;

            if (!LastTaken.HasValue)
                return;

            var timeoutedAgents = _timesTaken.Where(x => (_gc.WorldTime.Value - x).TotalMinutes > _gameMinutesAgentIsActive).ToList();

            timeoutedAgents.ForEach(x => _timesTaken.Remove(x));
        }

        public void OnApplianceTaken(InventoryItemBase applianceItem, Player.BodyParts bodyPart)
        {
            if (_group.IsApplicableToGroup(applianceItem))
            {
                _timesTaken.Add(_gc.WorldTime.Value);
            }
        }

        public void OnConsumeItem(InventoryConsumableItemBase item)
        {
            var food = item as FoodItemBase;

            var isSpoiled = food != null && food.IsSpoiled;

            if (_group.IsApplicableToGroup(item) && !isSpoiled)
            {
                _timesTaken.Add(_gc.WorldTime.Value);
            }
        }

        private void BuildCurve()
        {
            if (_activationType == CurveTypes.ActiveImmediately)
            {
                _activationCurve = new MultiKeyedLerp(
                    new Keyframe(0f, 0f),
                    new Keyframe(5f, 100f),
                    new Keyframe(56f, 97f),
                    new Keyframe(100f, 0f)
                );
            }

            if (_activationType == CurveTypes.ActiveInSecondHalf)
            {
                _activationCurve = new MultiKeyedLerp(
                    new Keyframe(0f, 0f),
                    new Keyframe(50f, 5f),
                    new Keyframe(56f, 100f),
                    new Keyframe(84f, 96f),
                    new Keyframe(100f, 0f)
                );
            }

            if (_activationType == CurveTypes.SlowActivation)
            {
                _activationCurve = new MultiKeyedLerp(
                    new Keyframe(0f, 0f),
                    new Keyframe(32f, 100f),
                    new Keyframe(100f, 0f)
                );
            }
        }

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new ActiveMedicalAgentSnippet
            {
                GameMinutesAgentIsActive = _gameMinutesAgentIsActive,
                TimesTaken = _timesTaken
            };

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (ActiveMedicalAgentSnippet)savedState;

            _gameMinutesAgentIsActive = state.GameMinutesAgentIsActive;
            _timesTaken = state.TimesTaken;
        }

        #endregion 

    }

    public interface IActiveMedicalAgent
    {
        void Check();
        void OnApplianceTaken(InventoryItemBase applianceItem, Player.BodyParts bodyPart);
        void OnConsumeItem(InventoryConsumableItemBase item);
        bool IsActive { get; }
        int ActiveDosesCount { get; }
        DateTime? LastTaken { get; }
        MedicalConsumablesGroup MedicalGroup { get; }
        float PercentOfActivity { get; }
        float PercentOfPresence { get; }
    }

}
