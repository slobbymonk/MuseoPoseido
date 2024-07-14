using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Ghost.Ghost;

namespace Ghost.States
{
    public class IdleState : GhostState
    {
        public IdleState(Ghost ghost, GhostStateMachine stateMachine) : base(ghost, stateMachine)
        {

        }

        public override void AnimationTriggerEvent(Ghost.AnimationTriggerType animationTriggerType)
        {
            base.AnimationTriggerEvent(animationTriggerType);

            _ghost.GhostIdleSOBInstance.DoAnimationTriggerEventLogic(animationTriggerType);
        }

        public override void EnterState()
        {
            base.EnterState();

            _ghost.GhostIdleSOBInstance.DoEnterLogic();
        }

        public override void ExitState()
        {
            base.ExitState();

            _ghost.GhostIdleSOBInstance.DoExitLogic();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            _ghost.GhostIdleSOBInstance.DoFrameUpdateLogic();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            _ghost.GhostIdleSOBInstance.DoPhyiscsLogic();
        }
    }
}
