using System;
using System.Globalization;

namespace wLib.Procedure
{
    public abstract class GameProcedure<TProcedureIndex> : ProcedureBase<ProcedureController<TProcedureIndex>>
        where TProcedureIndex : struct, IConvertible
    {
        public abstract TProcedureIndex Index { get; }

        public override void SetContext(ProcedureController<TProcedureIndex> context)
        {
            base.SetContext(context);
            Init(context);
        }

        public sealed override void Enter()
        {
            base.Enter();
            Enter(Context);
        }

        public sealed override void Exit()
        {
            base.Exit();
            Exit(Context);
        }

        public sealed override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            Update(Context, deltaTime);
        }

        public virtual void Init(ProcedureController<TProcedureIndex> controller) { }

        public virtual void Enter(ProcedureController<TProcedureIndex> controller) { }

        public virtual void Exit(ProcedureController<TProcedureIndex> controller) { }

        public virtual void Update(ProcedureController<TProcedureIndex> controller, float deltaTime) { }

        #region Facade

        public void ChangeState(TProcedureIndex index)
        {
            Context.ChangeState(index.ToString(CultureInfo.InvariantCulture));
        }

        public void PushState(TProcedureIndex index)
        {
            Context.PushState(index.ToString(CultureInfo.InvariantCulture));
        }

        public new void ChangeState(string stateName)
        {
            Context.ChangeState(stateName);
        }

        public new void PushState(string stateName)
        {
            Context.PushState(stateName);
        }

        public new void PopState()
        {
            Context.PopState();
        }

        public new void TriggerEvent(string eventId, EventArgs args)
        {
            Context.TriggerEvent(eventId, args);
        }

        #endregion
    }
}