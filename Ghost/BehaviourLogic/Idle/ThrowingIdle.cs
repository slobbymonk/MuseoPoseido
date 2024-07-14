using Ghost.States;
using UnityEngine;
using UnityEngine.AI;

namespace Ghost
{
    [CreateAssetMenu(fileName = "Throwing-Idle", menuName = "Ghost-States/Idle/Throwing-Idle")]
    public class ThrowingIdle : GhostIdleSOB
    {
        public float _roamingRange;

        public float _moveSpeed;

        public float _forwardRotationCorrection = 0f;

        Thrower throwingGhost;

        public float _damageToPossessingItem;

        public float _delayBetweenThrows = 3;
        private float _delayed;

        public override void DoAnimationTriggerEventLogic(Ghost.AnimationTriggerType animationTriggerType)
        {
            base.DoAnimationTriggerEventLogic(animationTriggerType);
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();

            throwingGhost = (Thrower)_ghost;

            Vector3 point;
            if (GetRandomPoint(_transform.position, _roamingRange, out point))
            {
                _agent.SetDestination(point);
            }
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
            throwingGhost._readyToThrowParticle.SetActive(false);
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

            if (throwingGhost._targetItem != null)
            {
                if (throwingGhost._targetItem.GetPossessionState())
                {
                    if(throwingGhost._canThrow && CanSeePlayer())
                        ThrowPossessedItem();
                    if (!throwingGhost._isReadyToThrow) 
                        HandleThrowingDelay();
                }
            }
            else
            {
                if (throwingGhost._itemHoldingPosition.childCount == 0)
                    throwingGhost.FindTarget();
                else
                 throwingGhost._targetItem = throwingGhost._itemHoldingPosition.GetChild(0).gameObject.GetComponent<Artifacts>();
            }
        }
        bool CanSeePlayer()
        {
            var directionToPlayer = _playerTransform.position - _transform.position;

            RaycastHit hit;
            if(Physics.Raycast(_transform.position, directionToPlayer, out hit, 999, ~throwingGhost._ignoreLayersForPlayerSearch))
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Debug.Log("Failed to check if player was there");
                return false;
            }
        }
        void HandleThrowingDelay()
        {
            if (_delayBetweenThrows <= _delayed)
            {
                throwingGhost._isReadyToThrow = true;
            }
            else
            {
                throwingGhost._isReadyToThrow = false;
                _delayed += Time.deltaTime;
            }
        }
        public void ThrowPossessedItem()
        {
            _delayed = 0;
            throwingGhost._isReadyToThrow = false;
            throwingGhost._canThrow = false;
            throwingGhost._readyToThrowParticle.SetActive(false);

            throwingGhost._targetItem.GetThrownedIntoTheAir();

            Vector3 targetPosition = _playerTransform.position;

            targetPosition = new Vector3(targetPosition.x, targetPosition.y + 8, targetPosition.z);
            Vector3 dir = targetPosition - throwingGhost._targetItem.transform.position;

            throwingGhost._targetItem.GetComponent<Rigidbody>().AddForce(dir * 0.7f, ForceMode.VelocityChange);
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
