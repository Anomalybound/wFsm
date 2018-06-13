using UnityEngine;

namespace wFSM.Unity
{
    public abstract class FsmOwner : MonoBehaviour
    {
        private IState _root;
        private bool _running;

        public abstract IState BuildState();

        protected virtual void Awake()
        {
            _root = BuildState();
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
            if (_running) { _root.Update(Time.deltaTime); }
        }
    }
}