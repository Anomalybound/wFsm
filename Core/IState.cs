using System;

namespace wFSM
{
    public interface IState
    {
        #region Properties

        IState Parent { get; set; }

        #endregion

        #region Lifetime

        void Enter();

        void Update(float deltaTime);

        void Exit();

        #endregion

        #region Operations

        void ChangeState(string name);

        void PushState(string name);

        void PopState(string name);

        void TriggerEvent(string id, EventArgs eventArgs);

        #endregion
    }
}