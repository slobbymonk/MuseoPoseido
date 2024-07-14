using Ghost.States;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

namespace Ghost
{
    [CreateAssetMenu(fileName = "Searching-Item", menuName = "Ghost-States/Searching/Searching-Item")]
    public class PossessionSearchingBehaviour : GhostSearchingSOB
    {
        IArtifactOriented _itemOrientedGhost;

        public float _moveSpeed = 3;

        public override void DoAnimationTriggerEventLogic(Ghost.AnimationTriggerType animationTriggerType)
        {
            base.DoAnimationTriggerEventLogic(animationTriggerType);
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();

            _itemOrientedGhost = (IArtifactOriented)_ghost;

            if (_itemOrientedGhost._targetItem == null && _itemOrientedGhost._itemHoldingPosition.childCount == 0)
                _itemOrientedGhost.FindTarget();
            else if (_itemOrientedGhost._targetItem == null)
                _itemOrientedGhost._targetItem = _itemOrientedGhost._itemHoldingPosition.GetChild(0).gameObject.GetComponent<Artifacts>();

            _agent.SetDestination(_itemOrientedGhost._targetItem.transform.position);
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
            //Set target to item
            if (_itemOrientedGhost._targetItem != null)
            {
                //Find new target when target already possessed
                if (!_itemOrientedGhost.CheckIfNotAlreadyPossessed(_itemOrientedGhost._targetItem.gameObject))
                {
                    _itemOrientedGhost.FindTarget();
                    _agent.SetDestination(_itemOrientedGhost._targetItem.transform.position);
                }
                //Possess Item when close enough to it
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                {
                    _itemOrientedGhost.PossessItem();
                }
            }
            else
            {
                //Debug.Log("No target item");
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