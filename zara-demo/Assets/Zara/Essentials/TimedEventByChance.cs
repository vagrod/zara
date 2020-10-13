using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine
{
    public class TimedEventByChance : IGameEventByChance
    {

        private readonly Action<TimedEventByChance> _eventStartHappenningAction;
        private readonly Action<TimedEventByChance> _eventEndHappenningAction;
        private int _chanceOfHappening;
        private readonly float _realSecondsBetweenChecks;
        private readonly string _name;
        private readonly float _updateRate;
        private readonly IGameController _gc;

        private IGameEvent _chainedEvent;
        private IGameEvent _rootEvent;
        private IGameEvent _parentEvent;

        private float _gameSecondsSinceEventStarted;
        private DateTime _previousWorldTime;

        public Guid Id { get; set; }
        public bool IsHappened { get; set; }
        public bool IsEnded { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan RealDuration { get; private set; }
        public Pair<int> DurationPercentRandomizer { get; set; }

        public bool AutoReset { get; set; }

        private float _coundownTimer;

        public TimedEventByChance(IGameController gc, string name, Action<TimedEventByChance> eventStartHappenningAction, Action<TimedEventByChance> eventEndHappenningAction, TimeSpan eventDurationInGameTime, Pair<int> durationPercentRandomizer, int chanceOfHappening, float realSecondsBetweenChecks, float updateRate)
        {
            _gc = gc;
            _name = name;

            Duration = eventDurationInGameTime;
            RealDuration = Duration;
            DurationPercentRandomizer = durationPercentRandomizer;

            _eventStartHappenningAction = eventStartHappenningAction;
            _eventEndHappenningAction = eventEndHappenningAction;
            _chanceOfHappening = chanceOfHappening;
            _realSecondsBetweenChecks = realSecondsBetweenChecks;
            _updateRate = updateRate;

            _coundownTimer = _realSecondsBetweenChecks;
        }


        public TimedEventByChance(IGameController gc, string name, Action<TimedEventByChance> eventStartHappenningAction, Action<TimedEventByChance> eventEndHappenningAction, TimeSpan eventDurationInGameTime, Pair<int> durationPercentRandomizer, int chanceOfHappening, float realSecondsBetweenChecks) : this (gc, name, eventStartHappenningAction, eventEndHappenningAction, eventDurationInGameTime, durationPercentRandomizer, chanceOfHappening, realSecondsBetweenChecks, 0)
        {
            
        }

        public TimedEventByChance(IGameController gc, string name, Action<TimedEventByChance> eventStartHappenningAction, Action<TimedEventByChance> eventEndHappenningAction, TimeSpan eventDurationInGameTime, float realSecondsBetweenChecks, float updateRate) : this(gc, name, eventStartHappenningAction, eventEndHappenningAction, eventDurationInGameTime, null, 0, realSecondsBetweenChecks, updateRate)
        {
            
        }

        public TimedEventByChance(IGameController gc, string name, Action<TimedEventByChance> eventStartHappenningAction, Action<TimedEventByChance> eventEndHappenningAction, TimeSpan eventDurationInGameTime, Pair<int> durationPercentRandomizer, float realSecondsBetweenChecks, float updateRate) : this(gc, name, eventStartHappenningAction, eventEndHappenningAction, eventDurationInGameTime, durationPercentRandomizer, 0, realSecondsBetweenChecks, updateRate)
        {

        }

        public TimedEventByChance(IGameController gc, string name, Action<TimedEventByChance> eventStartHappenningAction, Action<TimedEventByChance> eventEndHappenningAction, TimeSpan eventDurationInGameTime, float realSecondsBetweenChecks) : this(gc, name, eventStartHappenningAction, eventEndHappenningAction, eventDurationInGameTime, null, 0, realSecondsBetweenChecks, 0)
        {

        }

        public TimedEventByChance(IGameController gc, string name, Action<TimedEventByChance> eventStartHappenningAction, Action<TimedEventByChance> eventEndHappenningAction, TimeSpan eventDurationInGameTime, Pair<int> durationPercentRandomizer, float realSecondsBetweenChecks) : this(gc, name, eventStartHappenningAction, eventEndHappenningAction, eventDurationInGameTime, durationPercentRandomizer, 0, realSecondsBetweenChecks, 0)
        {

        }

        public IGameEvent ChainedEvent
        {
            get { return _chainedEvent; }
            set
            {
                if (_chainedEvent != value)
                {
                    _chainedEvent = value;

                    if (value != null)
                    {
                        if (RootEvent == null)
                            value.AddChainNode(this, this);
                        else
                            value.AddChainNode(this, RootEvent);
                    }
                }
            }
        }

        void IGameEvent.AddChainNode(IGameEvent parent, IGameEvent root)
        {
            _rootEvent = root;
            _parentEvent = parent;
        }

        public IGameEvent RootEvent
        {
            get { return _rootEvent; }
        }

        public IGameEvent ParentEvent
        {
            get { return _parentEvent; }
        }

        public void Reset()
        {
            if (!IsHappened && !IsEnded)
                return;

            IsHappened = false;
            IsEnded = false;

            _previousWorldTime = default(DateTime);
            _gameSecondsSinceEventStarted = 0f;
            _coundownTimer = _realSecondsBetweenChecks;

            if (ChainedEvent != null)
                ChainedEvent.Reset();

            if (RootEvent != null)
                RootEvent.Reset();
        }

        public bool ChainEnded()
        {
            if (ChainedEvent == null)
                return IsHappened && IsEnded;

            return ChainedEvent.ChainEnded();
        }

        public bool IsActive { get {
            if (IsHappened && !IsEnded)
                return true;

            if (ChainedEvent != null)
                return ChainedEvent.IsActive;

            return false;
        } }

        public bool Check(float deltaTime)
        {
            if (_gc == null)
                return false;

            if (!_gc.WorldTime.HasValue)
                return false;

            if (IsHappened && IsEnded)
            {
                if (ChainedEvent != null)
                    return ChainedEvent.Check(deltaTime);
            }

            if (_coundownTimer < _realSecondsBetweenChecks)
            {
                if (Math.Abs(_updateRate) < 0.00001)
                    _coundownTimer += deltaTime;
                else 
                    _coundownTimer += _updateRate;
            }
            else
            {
                if (_coundownTimer >= _realSecondsBetweenChecks)
                    _coundownTimer = 0;

                if (!IsHappened)
                {
                    // Check for event chance
                    var willHappen = _chanceOfHappening.WillHappen();

                    if (willHappen)
                    {
                        IsHappened = true;

                        if (DurationPercentRandomizer != null)
                        {
                            var durationPart = Helpers.RollDice(DurationPercentRandomizer.First, DurationPercentRandomizer.Second) / 100f;
                            RealDuration = TimeSpan.FromSeconds(Duration.TotalSeconds * durationPart);

                            //(_name + " happened and will last for game's " + RealDuration.Hours + "h:" + RealDuration.Minutes + "m:" + RealDuration.Seconds + "s (until " + (_gc.WorldTime.Value + RealDuration).ToString("HH:mm") + ")");
                        }

                        _gameSecondsSinceEventStarted = 0;

                        if (_eventStartHappenningAction != null)
                            _eventStartHappenningAction.Invoke(this);
                    }
                }

                if (IsHappened && !IsEnded)
                {
                    if (_gameSecondsSinceEventStarted >= RealDuration.TotalSeconds)
                    {
                        IsEnded = true;

                        if (_eventEndHappenningAction != null)
                            _eventEndHappenningAction.Invoke(this);

                        //(_name + " ended");

                        if (AutoReset)
                            Reset();
                        else
                        {
                            //  Smooth transitions between chain nodes
                            if (ChainedEvent != null)
                                return ChainedEvent.Check(deltaTime);
                        } 
                    }
                    else
                    {
                        if (_previousWorldTime == default(DateTime))
                            _previousWorldTime = _gc.WorldTime.Value;

                        _gameSecondsSinceEventStarted += (float)(_gc.WorldTime.Value - _previousWorldTime).TotalSeconds;

                        _previousWorldTime = _gc.WorldTime.Value;
                    }
                }
            }

            return IsHappened && !IsEnded;
        }

        public bool Check(int chanceOfHappening, float deltaTime)
        {
            _chanceOfHappening = chanceOfHappening;

            return Check(deltaTime);
        }

        public void Debug_SetProbability(int value)
        {
            _chanceOfHappening = value;
        }

    }
}
