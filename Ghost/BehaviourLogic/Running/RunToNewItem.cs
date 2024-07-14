using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Ghost
{
    [CreateAssetMenu(fileName = "Running", menuName = "Ghost-States/Running/Running")]
    public class RunToNewItem : GhostRunningSOB
    {
        IArtifactOriented possesiveGhost;

        public float _distanceToRunFromPlayer;

        private float _timeBeforeSearchingAgain;

        public override void DoAnimationTriggerEventLogic(Ghost.AnimationTriggerType animationTriggerType)
        {
            base.DoAnimationTriggerEventLogic(animationTriggerType);
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();

            possesiveGhost = (IArtifactOriented)_ghost;
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();

            Vector3 randomPointOnSphere = Random.onUnitSphere;
            Vector3 randomPoint = _transform.position + randomPointOnSphere * _distanceToRunFromPlayer;

            _agent.SetDestination(randomPoint);

            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _ghost.StateMachine.ChangeState(_ghost.SearchingState);
            }
        }
        public override void DoPhyiscsLogic()
        {
            base.DoPhyiscsLogic();
        }

        public override void Initialize(GameObject gameObject, Ghost ghost, NavMeshAgent agent)
        {
            base.Initialize(gameObject, ghost, agent);
        }

        public override void ResetValues()
        {
            base.ResetValues();
        }
    }
}