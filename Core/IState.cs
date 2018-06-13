using System;
using System.Collections.Generic;

namespace wLib.Fsm
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

        void PushState(string name);

        void PopState();

        void TriggerEvent(string id, EventArgs eventArgs);

        #endregion
    }
}