using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Ghost
{
    public class GhostRunningSOB : ScriptableObject
    {
        protected Ghost _ghost;
        protected Transform _transform;
        protected GameObject _gameObject;

        protected Transform _playerTransform;

        protected NavMeshAgent _agent;

        public virtual void Initialize(GameObject gameObject, Ghost ghost, NavMeshAgent agent)
        {
            _ghost = ghost;
            _transform = gameObject.transform;
            _gameObject = gameObject;
            _agent = agent;

            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public virtual void DoEnterLogic() { }
        public virtual void DoExitLogic() { ResetValues(); }
        public virtual void DoFrameUpdateLogic()
        {
            //TODO: If it's within distance of the item it's running towards then it will take over that item & switch to the idle state
        }
        public virtual void DoPhyiscsLogic() { }
        public virtual void DoAnimationTriggerEventLogic(Ghost.AnimationTriggerType animationTriggerType) { }
        public virtual void ResetValues() { }
    }
}