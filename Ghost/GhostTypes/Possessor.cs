using Ghost.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ghost
{
    public class Possessor : Ghost, ISuckable, IArtifactOriented
    {
        [field: SerializeField] public Artifacts _targetItem { get; set; }
        [field: SerializeField] public float _findItemRange { get; set; }
        [field: SerializeField] public LayerMask _ignoreLayersForPlayerSearch { get; set; }
        [field: SerializeField] public Transform _itemHoldingPosition { get; set; }

        public GameObject _possessingEffect;

        public void FindTarget()
        {
            var allPotentialItems = GameObject.FindGameObjectsWithTag("Item");
            var finalItems = new List<GameObject>();

            foreach (var item in allPotentialItems)
            {
                if(CheckIfNotAlreadyPossessed(item) && Vector3.Distance(transform.position, item.transform.position) < _findItemRange)
                {
                    finalItems.Add(item);
                }
            }
            if(finalItems.Count > 0)
            {
                int randomItem = Random.Range(0, finalItems.Count);
                _targetItem = finalItems[randomItem].GetComponent<Artifacts>();
            }
        }

        public bool CheckIfNotAlreadyPossessed(GameObject item)
        {
            if (item.GetComponent<Artifacts>().GetPossessionState())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void PossessItem()
        {
            if (_targetItem != null && _itemHoldingPosition.childCount == 0)
            {
                _targetItem.WhenPossessed(_itemHoldingPosition);
                ChangeGhostAndItem(false);
            }
            StateMachine.ChangeState(IdleState);
        }

        private void ChangeGhostAndItem(bool activateOrNot)
        {
            if(_possessingEffect != null) _possessingEffect.SetActive(!activateOrNot);
            transform.GetChild(0).gameObject.SetActive(activateOrNot);
        }

        #region Sucking Logic
        public void IsBeingSucked()
        {
            if (_targetItem != null)
            {
                ChangeGhostAndItem(true);
                _targetItem.WhenPossessorBeingSucked();
            }
            StateMachine.ChangeState(StrugglingState);
        }

        public void ResetSuck()
        {
            /*if (_targetItem != null && !_targetItem.GetPossessionState())
            {
                _targetItem.WhenReleasedFromPossession();
            }*/ // Moved this to Capture & changed searching so that it feels more fair, the ghost won't drop the item unless captured
            StateMachine.ChangeState(RunningState);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
        public void Capture()
        {
            if (_targetItem != null && !_targetItem.GetPossessionState())
            {
                _targetItem.WhenReleasedFromPossession();
            }
            Destroy(gameObject);
        }

        #endregion

        /*private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && _targetItem == null && StateMachine.CurrentEnemyState != StrugglingState
                 && StateMachine.CurrentEnemyState != SearchingState)
            {
                RaycastHit hit;
                Vector3 dir = other.transform.position - transform.position;
                if (Physics.Raycast(transform.position, dir, out hit))
                {
                    Debug.Log(hit.transform.gameObject.name);
                    if (hit.transform == other.transform)
                        StateMachine.ChangeState(SearchingState);
                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _findItemRange);
        }*/
    }
}
