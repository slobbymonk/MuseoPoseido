using Ghost;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ghost.States
{
    public class SearchingState : GhostState
    {
        public SearchingState(Ghost ghost, GhostStateMachine stateMachine) : base(ghost, stateMachine)
        {

        }

        public override void AnimationTriggerEvent(Ghost.AnimationTriggerType animationTriggerType)
        {
            base.AnimationTriggerEvent(animationTriggerType);
            _ghost.GhostSearchingSOBInstance.DoAnimationTriggerEventLogic(animationTriggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            _ghost.GhostSearchingSOBInstance.DoEnterLogic();
        }

        public override void ExitState()
        {
            base.ExitState();
            _ghost.GhostSearchingSOBInstance.DoExitLogic();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            _ghost.GhostSearchingSOBInstance.DoFrameUpdateLogic();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            _ghost.GhostSearchingSOBInstance.DoPhyiscsLogic();
        }
    }

}
