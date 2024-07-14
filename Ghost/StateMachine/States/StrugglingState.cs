using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ghost.States
{
    public class StrugglingState : GhostState
    {
        public StrugglingState(Ghost ghost, GhostStateMachine stateMachine) : base(ghost, stateMachine)
        {
        }

        public override void AnimationTriggerEvent(Ghost.AnimationTriggerType animationTriggerType)
        {
            base.AnimationTriggerEvent(animationTriggerType);
            _ghost.GhostStrugglingSOBInstance.DoAnimationTriggerEventLogic(animationTriggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            _ghost.GhostStrugglingSOBInstance.DoEnterLogic();
        }

        public override void ExitState()
        {
            base.ExitState();
            _ghost.GhostStrugglingSOBInstance.DoExitLogic();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            _ghost.GhostStrugglingSOBInstance.DoFrameUpdateLogic();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            _ghost.GhostStrugglingSOBInstance.DoPhyiscsLogic();
        }
    }
}
