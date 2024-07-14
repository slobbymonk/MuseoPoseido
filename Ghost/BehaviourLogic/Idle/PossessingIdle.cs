using Ghost.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Ghost
{
    [CreateAssetMenu(fileName = "Possessing-Idle", menuName = "Ghost-States/Idle/Possessing-Idle")]
    public class PossessingIdle : GhostIdleSOB
    {
        public float _roamingRange;

        public float _moveSpeed;

        public float _forwardRotationCorrection = 0f;

        IArtifactOriented possesiveGhost;

        public float _damageToPossessingItem;

        public float _delayBetweenDamage = 1;
        private float _delayed;

        public override void DoAnimationTriggerEventLogic(Ghost.AnimationTriggerType animationTriggerType)
        {
            base.DoAnimationTriggerEventLogic(animationTriggerType);
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();

            possesiveGhost = (IArtifactOriented)_ghost;

            Vector3 point;
            if (GetRandomPoint(_transform.position, _roamingRange, out point))
            {
                _agent.SetDestination(point);
            }
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }

        public override void DoFrameUpdateLogic()
        {
            //Make ghost look towards where it's going

            var yRotation = Mathf.Atan2(_agent.velocity.x, _agent.velocity.z) * Mathf.Rad2Deg;

            _ghost.transform.localEulerAngles = new Vector3(0, yRotation + _forwardRotationCorrection, 0);

            //Check if ghost is at destination
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                Vector3 point;
                if (GetRandomPoint(_transform.position, _roamingRange, out point))
                {
                    _agent.SetDestination(point);
                }
            }
            if (possesiveGhost._targetItem != null)
            {
                if (possesiveGhost._targetItem.GetPossessionState())
                {
                    DamagePossessingItem();
                }
            }

            //Check if P has been pressed to look for item
            if (Input.GetKeyDown(KeyCode.P))
            {
                _ghost.StateMachine.ChangeState(_ghost.SearchingState);
            }
        }
        public void DamagePossessingItem()
        {
            if (_delayBetweenDamage <= _delayed)
            {
                possesiveGhost._targetItem.TakeDamage(_damageToPossessingItem);
                _delayed = 0;
            }
            else _delayed += Time.deltaTime;
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
        bool GetRandomPoint(Vector3 center, float range, out Vector3 result)
        {
            //Gives a random point as out result and returns a boolean based on whether or not it was able to get a point
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
            result = Vector3.zero;
            return false;
        }
    }

}