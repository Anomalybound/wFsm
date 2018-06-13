﻿namespace wFsm.Builder
{
    public class StateMachineBuilder<TState> where TState : StateBase, new()
    {
        private StateBase _root;

        public StateMachineBuilder()
        {
            _root = new TState();
        }

        #region Fluent Actions

        public StateBuilder<TState, StateMachineBuilder<TState>> State(string stateName)
        {
            return new StateBuilder<TState, StateMachineBuilder<TState>>(stateName, this, _root);
        }

        public StateBuilder<T, StateMachineBuilder<TState>> State<T>(string stateName) where T : StateBase, new()
        {
            return new StateBuilder<T, StateMachineBuilder<TState>>(stateName, this, _root);
        }

        public StateBase Build()
        {
            return _root;
        }

        #endregion
    }
}