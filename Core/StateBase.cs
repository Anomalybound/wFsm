using System;
using System.Collections.Generic;

namespace wLib.Fsm
{
    public abstract class StateBase : IState
    {
        #region IState Implementation

        public IState Parent { get; set; }

        public float ElapsedTime { get; private set; }

        public Dictionary<string, IState> Children
        {
            get { return _children; }
        }

        public Stack<IState> ActiveStates
        {
            get { return _activeStates; }
        }

        public virtual void Enter()
        {
            if (OnEnter != null) { OnEnter.Invoke(); }

            ElapsedTime = 0f;
        }

        public virtual void Update(float deltaTime)
        {
            // Only update the lastest state
            if (_activeStates.Count > 0)
            {
                _activeStates.Peek().Update(deltaTime);
                return;
            }

            if (OnUpdate != null) { OnUpdate.Invoke(deltaTime); }

            ElapsedTime += deltaTime;

            // Check if condition meets
            foreach (var conditionPair in _conditions)
            {
                if (conditionPair.Key.Invoke() && conditionPair.Value != null) { conditionPair.Value.Invoke(); }
            }
        }

        public virtual void Exit()
        {
            if (OnExit != null) { OnExit.Invoke(); }
        }

        public virtual void ChangeState(string name)
        {
            IState result;
            if (!_children.TryGetValue(name, out result))
            {
                throw new ApplicationException(string.Format("Child state [{0}] not found.", name));
            }

            if (_activeStates.Count > 0) { PopState(); }

            PrivatePushState(result);
        }

        public void PushState(string name)
        {
            IState result;
            if (!_children.TryGetValue(name, out result))
            {
                throw new ApplicationException(string.Format("Child state [{0}] not found.", name));
            }

//            if (_activeStates.Contains(result))
//            {
//                throw new ApplicationException(string.Format("State [{0}] already in stack.", name));
//            }

            PrivatePushState(result);
        }

        public void PopState()
        {
            PrivatePopState();
        }

        public void TriggerEvent(string id)
        {
            TriggerEvent(id, EventArgs.Empty);
        }

        public void TriggerEvent(string id, EventArgs eventArgs)
        {
            if (_activeStates.Count > 0)
            {
                _activeStates.Peek().TriggerEvent(id, eventArgs);
                return;
            }

            Action<EventArgs> action;
            if (!_events.TryGetValue(id, out action))
            {
                throw new ApplicationException(string.Format("Event [{0}] not exits.", id));
            }

            if (action != null) { action.Invoke(eventArgs); }
        }

        #endregion

        #region Actions

        public event Action OnEnter;
        public event Action OnExit;
        public event Action<float> OnUpdate;

        #endregion

        #region Runtime

        private readonly Stack<IState> _activeStates = new Stack<IState>();
        private readonly Dictionary<string, IState> _children = new Dictionary<string, IState>();
        private readonly Dictionary<string, Action<EventArgs>> _events = new Dictionary<string, Action<EventArgs>>();
        private readonly Dictionary<Func<bool>, Action> _conditions = new Dictionary<Func<bool>, Action>();

        #endregion

        #region Private Operations

        private void PrivatePopState()
        {
            var result = _activeStates.Pop();
            result.Exit();
        }

        private void PrivatePushState(IState state)
        {
            _activeStates.Push(state);
            state.Enter();
        }

        #endregion

        #region Helper

        public void AddChild(string name, IState state)
        {
            if (!_children.ContainsKey(name))
            {
                _children.Add(name, state);
                state.Parent = this;
            }
            else { throw new ApplicationException(string.Format("Child state already exists: {0}", name)); }
        }

        public void SetEnterAction(Action onEnter)
        {
            OnEnter = onEnter;
        }

        public void SetExitAction(Action onExit)
        {
            OnExit = onExit;
        }

        public void SetUpdateAction(Action<float> onUpdate)
        {
            OnUpdate = onUpdate;
        }

        public void AddEvent(string id, Action<EventArgs> action)
        {
            if (!_events.ContainsKey(id)) { _events.Add(id, action); }
            else { throw new ApplicationException(string.Format("Event already exists: {0}", id)); }
        }

        public void AddEvent<TArgs>(string id, Action<TArgs> action) where TArgs : EventArgs
        {
            if (!_events.ContainsKey(id)) { _events.Add(id, arg => { action.Invoke((TArgs) arg); }); }
            else { throw new ApplicationException(string.Format("Event already exists: {0}", id)); }
        }

        public void AddCondition(Func<bool> predicate, Action action)
        {
            _conditions.Add(predicate, action);
        }

        #endregion
    }
}