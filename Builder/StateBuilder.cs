using System;

namespace wFsm.Builder
{
    public class StateBuilder<TState, TParentBuilder> where TState : StateBase, new()
    {
        private readonly TState _state;
        private readonly TParentBuilder _builder;

        public StateBuilder(string name, TParentBuilder builder, StateBase parent)
        {
            _builder = builder;
            _state = new TState();
            parent.AddChild(name, _state);
        }

        #region Fluent Actions

        public StateBuilder<State, StateBuilder<TState, TParentBuilder>> State(string name)
        {
            return new StateBuilder<State, StateBuilder<TState, TParentBuilder>>(name, this, _state);
        }

        public StateBuilder<T, StateBuilder<TState, TParentBuilder>> State<T>(string name) where T : StateBase, new()
        {
            return new StateBuilder<T, StateBuilder<TState, TParentBuilder>>(name, this, _state);
        }

        public StateBuilder<TState, TParentBuilder> OnEnter(Action<TState> action)
        {
            _state.SetEnterAction(() => action(_state));
            return this;
        }

        public StateBuilder<TState, TParentBuilder> OnExit(Action<TState> action)
        {
            _state.SetExitAction(() => action(_state));
            return this;
        }

        public StateBuilder<TState, TParentBuilder> OnUpdate(Action<TState, float> action)
        {
            _state.SetUpdateAction(deltaTime => action(_state, deltaTime));
            return this;
        }

        public StateBuilder<TState, TParentBuilder> Condition(Func<bool> predicate, Action<TState> action)
        {
            _state.AddCondition(predicate, () => action(_state));
            return this;
        }

        public StateBuilder<TState, TParentBuilder> Event(string id, Action<TState, EventArgs> action)
        {
            _state.AddEvent(id, args => action(_state, args));
            return this;
        }

        public StateBuilder<TState, TParentBuilder> Event<TArgs>(string id, Action<TState, TArgs> action)
            where TArgs : EventArgs
        {
            _state.AddEvent(id, args => action(_state, (TArgs) args));
            return this;
        }

        public TParentBuilder End()
        {
            return _builder;
        }

        #endregion
    }
}