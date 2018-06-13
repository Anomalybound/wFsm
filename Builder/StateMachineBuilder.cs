namespace wFSM.Builder
{
    public class StateMachineBuilder
    {
        private StateBase _root;

        public StateMachineBuilder()
        {
            _root = new State();
        }

        #region Fluent Actions

        public StateBuilder<State, StateMachineBuilder> State(string stateName)
        {
            return new StateBuilder<State, StateMachineBuilder>(stateName, this, _root);
        }

        public StateBuilder<TState, StateMachineBuilder> State<TState>(string stateName) where TState : StateBase, new()
        {
            return new StateBuilder<TState, StateMachineBuilder>(stateName, this, _root);
        }

        public StateBase Build()
        {
            return _root;
        }

        #endregion
    }
}