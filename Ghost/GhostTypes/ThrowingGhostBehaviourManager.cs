using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ghost
{
    public class ThrowingGhostBehaviourManager : MonoBehaviour
    {
        public List<Thrower> _ghosts= new List<Thrower>();
        public List<Thrower> _readyGhosts = new List<Thrower>();

        public GhostManager _manager;

        public float _delayBetweenThrows = 2;
        private float _delayed;

        public void Awake()
        {
            _manager.FillThrowingGhostList += FillThrowingManagerList;
        }

        void FillThrowingManagerList(object sender, EventArgs args)
        {
            var ghosts = _manager._spawnedGhosts;
            foreach (var ghost in ghosts)
            {
                if (ghost.GetComponent<Thrower>() != null)
                    _ghosts.Add(ghost.GetComponent<Thrower>());
            }
        }
        private void Update()
        {
            if(_ghosts.Count > 0)
            {
                RefillReadyGhostsList();

                if (_readyGhosts.Count > 0)
                {
                    if (_delayed >= _delayBetweenThrows)
                    {
                        var randomInt = Random.Range(0, _readyGhosts.Count - 1);

                        if (_readyGhosts[randomInt] != null)
                        {
                            _readyGhosts[randomInt]._readyToThrowParticle.SetActive(true);
                            _readyGhosts[randomInt].ThrowItem();
                            _delayed = 0;
                        }
                    }
                    else
                    {
                        _delayed += Time.deltaTime;
                    }
                }
            }
        }

        private void RefillReadyGhostsList()
        {
            _readyGhosts.Clear();
            foreach (var ghost in _ghosts)
            {
                if (ghost._isReadyToThrow)
                    _readyGhosts.Add(ghost);
            }
        }
    }
}
