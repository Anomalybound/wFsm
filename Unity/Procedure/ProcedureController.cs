using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using wLib.Fsm;

namespace wLib.Procedure
{
    public abstract class ProcedureController<TProcedureIndex> : FsmContainer, IProcedureController
        where TProcedureIndex : struct, IConvertible
    {
        private Dictionary<TProcedureIndex, GameProcedure<TProcedureIndex>> Indices =
            new Dictionary<TProcedureIndex, GameProcedure<TProcedureIndex>>();

        public TProcedureIndex Current;

        public override IState BuildState()
        {
            var root = new State();

            var types = GetType().Assembly.GetTypes()
                .Where(x => typeof(GameProcedure<TProcedureIndex>).IsAssignableFrom(x));

            var instances = new List<GameProcedure<TProcedureIndex>>();
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type) as GameProcedure<TProcedureIndex>;
                instance.SetContext(this);
                instances.Add(instance);
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