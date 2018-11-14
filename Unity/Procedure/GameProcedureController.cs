using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using wLib.Fsm;

namespace wLib.Procedure
{
    public abstract class GameProcedureController<TProcedureController, TProcedureIndex> : FsmContainer,
        IProcedureController
        where TProcedureController : GameProcedureController<TProcedureController, TProcedureIndex>
        where TProcedureIndex : struct, IConvertible
    {
        private Dictionary<TProcedureIndex, GameProcedure<TProcedureController, TProcedureIndex>> Indices =
            new Dictionary<TProcedureIndex, GameProcedure<TProcedureController, TProcedureIndex>>();

        private readonly Dictionary<IState, TProcedureIndex> IndexLookup = new Dictionary<IState, TProcedureIndex>();

        public TProcedureIndex Current => IndexLookup[Root.ActiveStates.Peek()];

        public override IState BuildState()
        {
            var root = new State();

            var types = GetType().Assembly.GetTypes()
                .Where(x => typeof(GameProcedure<TProcedureController, TProcedureIndex>).IsAssignableFrom(x));

            var instances = new List<GameProcedure<TProcedureController, TProcedureIndex>>();
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type) as GameProcedure<TProcedureController, TProcedureIndex>;
                if (instance != null)
                {
                    instance.SetContext((TProcedureController) this);
                    instances.Add(instance);
                }
            }

            instances = instances.OrderBy(x => x.Index).ToList();

            for (var i = 0; i < instances.Count; i++)
            {
                var instance = instances[i];
                var id = instance.Index;

                if (Indices.ContainsKey(id))
                {
                    Debug.LogErrorFormat("{0}[{1}] already added.", id, instance.GetType().Name);
                    continue;
                }

                Indices.Add(id, instance);
                IndexLookup.Add(instance, id);
                root.AddChild(id.ToString(CultureInfo.InvariantCulture), instance);
            }

            Root = root;
            if (instances.Count > 0) { ChangeState(instances[0].Index); }

            return Root;
        }

        #region Facade

        public void ChangeState(TProcedureIndex index)
        {
            Root.ChangeState(index.ToString(CultureInfo.InvariantCulture));
        }

        public void PushState(TProcedureIndex index)
        {
            Root.PushState(index.ToString(CultureInfo.InvariantCulture));
        }

        public void ChangeState(string stateName)
        {
            Root.ChangeState(stateName);
        }

        public void PushState(string stateName)
        {
            Root.PushState(stateName);
        }

        public void PopState()
        {
            Root.PopState();
        }

        public void TriggerEvent(string eventId, EventArgs args)
        {
            Root.TriggerEvent(eventId, args);
        }

        #endregion
    }
}