using Ghost.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Ghost.Ghost;

namespace Ghost.States {
    public class AttackingState : GhostState
    {
        public AttackingState(Ghost ghost, GhostStateMachine stateMachine) : base(ghost, stateMachine)
        {

        }

        public override void AnimationTriggerEvent(Ghost.AnimationTriggerType animationTriggerType)
        {
            base.AnimationTriggerEvent(animationTriggerType);

            _ghost.GhostAttackSOBInstance.DoAnimationTriggerEventLogic(animationTriggerType);
        }

        public override void EnterState()
        {
            base.EnterState();

            _ghost.GhostAttackSOBInstance.DoEnterLogic();
        }

        public override void ExitState()
        {
            base.ExitState();
            _ghost.GhostAttackSOBInstance.DoExitLogic();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            _ghost.GhostAttackSOBInstance.DoFrameUpdateLogic();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            _ghost.GhostAttackSOBInstance.DoPhyiscsLogic();
        }
    }
}
