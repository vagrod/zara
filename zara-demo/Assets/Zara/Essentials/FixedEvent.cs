using System;
using ZaraEngine.StateManaging;

namespace ZaraEngine
{
    public class FixedEvent : IGameEvent
    {

        private readonly string _name;
        private readonly Action<FixedEvent> _eventAction;

        private IGameEvent _chainedEvent;
        private IGameEvent _rootEvent;
        private IGameEvent _parentEvent;

        public FixedEvent(string name, Action<FixedEvent> eventAction)
        {
            _name = name;
            _eventAction = eventAction;
        }

        public Guid Id { get; set; }
        public bool IsHappened { get; set; }
        public bool AutoReset { get; set; }

        public IGameEvent ChainedEvent
        {
            get { return _chainedEvent; }
            set {
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
            return Invoke(deltaTime);
        }

        public void Reset()
        {
            if (!IsHappened)
                return;

            IsHappened = false;

            //(_name + " reset");

            if (ChainedEvent != null)
                ChainedEvent.Reset();

            if (RootEvent != null)
                RootEvent.Reset();
        }

        public bool Invoke(float deltaTime)
        {
            if (IsHappened)
            {
                if (ChainedEvent != null)
                    return ChainedEvent.Check(deltaTime);

                return false;
            }

            //(_name);

            if(_eventAction != null)
                _eventAction.Invoke(this);

            if (AutoReset)
                Reset();

            return false;
        }

        #region State Manage

        public IStateSnippet GetState()
        {
            var state = new FixedEventSnippet
            {
                IsHappened = this.IsHappened,
                AutoReset = this.AutoReset
            };

            return state;
        }

        public void RestoreState(IStateSnippet savedState)
        {
            var state = (FixedEventSnippet)savedState;

            IsHappened = state.IsHappened;
            AutoReset = state.AutoReset;
        }

        #endregion 

    }
}
