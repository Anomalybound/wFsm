using UnityEngine;
using wLib.Fsm;

namespace wLib
{
    public abstract class FsmContainer : MonoBehaviour
    {
        public IState Root { get; protected set; }

        private bool _running;

        public abstract IState BuildState();

        protected virtual void Awake()
        {
            Root = BuildState();
        }

        protected virtual void OnEnable()
        {
            _running = true;
        }

        protected virtual void OnDisable()
        {
            _running = false;
        }

        protected virtual void Update()
        {
            if (_running) { Root.Update(Time.deltaTime); }
        }
    }
}