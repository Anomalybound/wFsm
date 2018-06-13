namespace wFSM
{
    public interface IPureState
    {
        #region Lifetime

        void Enter();

        void Update(float deltaTime);

        void Exit();

        #endregion
    }
}