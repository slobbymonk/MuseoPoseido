using Ghost.States;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Ghost
{
    public class Thrower : Ghost, ISuckable, IArtifactOriented
    {
        [field: SerializeField] public Artifacts _targetItem { get; set ; }

        [field: SerializeField] public float _findItemRange { get; set; }

        [field: SerializeField] public LayerMask _ignoreLayersForPlayerSearch { get; set; }
        [field: SerializeField] public Transform _itemHoldingPosition { get; set; }

        [HideInInspector] public bool _isReadyToThrow;
        [HideInInspector] public bool _canThrow;

        public GameObject _readyToThrowParticle; //Exclamation mark
        public AudioClip _throwingSound;

        [SerializeField] private float _throwingDelayAfterQue = 2f;

        public void FindTarget()
        {
            var allPotentialItems = GameObject.FindGameObjectsWithTag("Item");
            var finalItems = new List<GameObject>();

            foreach (var item in allPotentialItems)
            {
                if (CheckIfNotAlreadyPossessed(item) && Vector3.Distance(transform.position, item.transform.position) < _findItemRange)
                {
                    finalItems.Add(item);
                }
            }
            if (finalItems.Count > 0)
            {
                int randomItem = UnityEngine.Random.Range(0, finalItems.Count);
                _targetItem = finalItems[randomItem].GetComponent<Artifacts>();
            }
        }
        public void ThrowItem()
        {
            Invoke("AllowToThrow", _throwingDelayAfterQue);
            Invoke("ChangeToSearching", _throwingDelayAfterQue+1);
        }
        void AllowToThrow()
        {
            _canThrow = true;
            _audioSource.clip = _throwingSound;
            _audioSource.Play();
        }
        void ChangeToSearching()
        {
            StateMachine.ChangeState(SearchingState);
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
            }
            StateMachine.ChangeState(IdleState);
        }


        #region Sucking Logic
        public void IsBeingSucked()
        {
            if (_targetItem != null)
            {
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
            StateMachine.ChangeState(IdleState);
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
    }

}