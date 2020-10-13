using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaraEngine
{
    public class EventByChance : IGameEventByChance
    {

        private readonly Action<EventByChance> _eventHappenedAction;
        private int _chanceOfHappening;
        private readonly float _realSecondsBetweenChecks;
        private readonly string _name;
        private readonly float _updateRate;

        private IGameEvent _chainedEvent;
        private IGameEvent _rootEvent;
        private IGameEvent _parentEvent;

        private float _coundownTimer;

        public Guid Id { get; set; }
        public bool IsHappened { get; set; }
        public bool AutoReset { get; set; }

        public object Param { get; set; }

        public EventByChance(string name, Action<EventByChance> eventHappenedAction, int chanceOfHappening, float realSecondsBetweenChecks, float updateRate)
        {
            _name = name;
            _eventHappenedAction = eventHappenedAction;
            _chanceOfHappening = chanceOfHappening;
            _realSecondsBetweenChecks = realSecondsBetweenChecks;
            _updateRate = updateRate;

            _coundownTimer = _realSecondsBetweenChecks;
        }

        public EventByChance(string name, Action<EventByChance> eventHappenedAction, float realSecondsBetweenChecks, float updateRate) : this(name, eventHappenedAction,  0, realSecondsBetweenChecks, updateRate)
        {
            
        }

        public EventByChance(string name, Action<EventByChance> eventHappenedAction, float realSecondsBetweenChecks) : this(name, eventHappenedAction, 0, realSecondsBetweenChecks, 0)
        {
            
        }

        public EventByChance(string name, Action<EventByChance> eventHappenedAction, int chanceOfHappening, float realSecondsBetweenChecks) : this(name, eventHappenedAction, chanceOfHappening, realSecondsBetweenChecks, 0)
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
            if (!IsHappened)
                return;

            IsHappened = false;
            _coundownTimer = _realSecondsBetweenChecks;

            if (ChainedEvent != null)
                ChainedEvent.Reset();

            if (RootEvent != null)
                RootEvent.Reset();
        }

        public bool Check(int chanceOfHappening, float deltaTime)
        {
            _chanceOfHappening = chanceOfHappening;

            return Check(deltaTime);
        }

        public bool ChainEnded()
        {
            if (ChainedEvent == null)
                return IsHappened;

            return ChainedEvent.ChainEnded();
        }

        public bool IsActive
        {
            get
            {
                if (!IsHappened)
                    return true;

                if (ChainedEvent != null)
                    return ChainedEvent.IsActive;

                return false;
            }
        }

        public bool Check(float deltaTime)
        {
            if (IsHappened)
            {
                if (ChainedEvent != null)
                    return ChainedEvent.Check(deltaTime);

                return false;
            }

            if (_coundownTimer < _realSecondsBetweenChecks)
            {
                if (Math.Abs(_updateRate) < 0.000001)
                    _coundownTimer += deltaTime;
                else 
                    _coundownTimer += _updateRate;
            }
            else
            {
                if (_coundownTimer >= _realSecondsBetweenChecks)
                    _coundownTimer = 0;

                //(_name);

                // Check for event chance
                var willHappen = _chanceOfHappening.WillHappen();

                if (willHappen)
                {
                    IsHappened = true;

                    if (_eventHappenedAction != null)
                        _eventHappenedAction.Invoke(this);

                    if(AutoReset)
                        Reset();
                }
            }

            return !IsHappened;
        }

    }
}
