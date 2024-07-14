using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ghost.States
{
    public class GhostState
    {
        protected Ghost _ghost;
        protected GhostStateMachine _stateMachine;

        public GhostState(Ghost ghost, GhostStateMachine stateMachine)
        {
            _ghost = ghost;
            _stateMachine = stateMachine;
        }

        public virtual void EnterState() { }
        public virtual void ExitState() { }
        public virtual void FrameUpdate() { }
        public virtual void PhysicsUpdate() { }
        public virtual void AnimationTriggerEvent(Ghost.AnimationTriggerType animationTriggerType) { }
    }
}