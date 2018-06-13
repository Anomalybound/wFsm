using System;
using System.Collections.Generic;

namespace wFSM
{
    public interface IState : IPureState
    {
        #region Properties

        IState Parent { get; set; }

        float ElapsedTime { get; }

        Dictionary<string, IState> Children { get; }

        Stack<IState> ActiveStates { get; }

        #endregion

        #region Operations

        void ChangeState(string name);

        void PrivatePushState(string name);

        void PopState();

        void TriggerEvent(string id, EventArgs eventArgs);

        #endregion
    }
}