using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ghost{ 
    public class GhostAttackSOB : ScriptableObject
    {
        protected Ghost _ghost;
        protected Transform _transform;
        protected GameObject _gameObject;
    
        protected Transform _playerTransform;

        public float _sanityDamage;
        public float _moveSpeed;

        public virtual void Initialize(GameObject gameObject, Ghost ghost)
        {
            _ghost = ghost;
            _transform = gameObject.transform;
            _gameObject = gameObject;
    
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    
        public virtual void DoEnterLogic() { }
        public virtual void DoExitLogic() { ResetValues(); }
        public virtual void DoFrameUpdateLogic()
        {
            //TODO: Attack logic
        }
        public virtual void DoPhyiscsLogic() { }
        public virtual void DoAnimationTriggerEventLogic(Ghost.AnimationTriggerType animationTriggerType) { }
        public virtual void ResetValues() { }
    }
}