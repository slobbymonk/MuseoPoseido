using Ghost;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ghost
{
    public class GhostManager : MonoBehaviour
    {
        private RoomManager _roomManager;

        public List<Ghost> _spawnedGhosts = new List<Ghost>();
        public List<GhostSpawnPoints> _spawnPoints;

        private bool _roomHasStarted;

        public event EventHandler FillThrowingGhostList;


        [Header("Sounds")]
        [Range(0.0f, 100.0f)]
        public float _soundChance;
        private float _actualChance;
        [Range(0.0f, 100.0f)]
        public float _chanceIncreasePerFail;

        public AudioClip[] _randomSounds;
        private float _startingPitch;

        private void Start()
        {
            _roomManager = GetComponent<RoomManager>();

            _roomManager.StartedRoom += SpawnGhosts;

            FindAllSpawnPoints();


            //Sounds
            StartCoroutine(PlayRandomIdleSound());
        }

        private void Update()
        {
            if (_roomHasStarted)
            {
                if (GhostsLeft() <= 0)
                {
                    _roomManager.ClearedRoom();
                }
            }
        }
        IEnumerator PlayRandomIdleSound()
        {
            int randomNumber = Random.Range(0, 100);
            
            if (randomNumber <= _actualChance)
            {
                int randomIndex = Random.Range(0, _randomSounds.Length);

                var audioSource = _spawnedGhosts[Random.Range(0, _spawnedGhosts.Count)].GetComponent<AudioSource>();

                audioSource.clip = _randomSounds[randomIndex];
                audioSource.pitch = _startingPitch * Random.Range(.95f, 1.05f);

                audioSource.Play();

                _actualChance = _soundChance;
            }
            else
            {
                _actualChance += _chanceIncreasePerFail;
            }

            yield return new WaitForSeconds(1);
            StartCoroutine(PlayRandomIdleSound());

            yield return null;
        }
        private int GhostsLeft()
        {
            int tempGhostAmount = 0;
            foreach (var ghost in _spawnedGhosts)
            {
                if (ghost != null)
                    tempGhostAmount++;
            }
            return tempGhostAmount;
        }
        private void FindAllSpawnPoints()
        {
            _spawnPoints = new List<GhostSpawnPoints>();

            var allSpawnPoints = GameObject.FindObjectsOfType<GhostSpawnPoints>();

            foreach (var spawnPoint in allSpawnPoints)
            {
                _spawnPoints.Add(spawnPoint);
            }
        }

        public void SpawnGhosts(object sender, EventArgs args)
        {
            _roomHasStarted = true;
            _spawnedGhosts = new List<Ghost>();
            foreach (var spawnPoint in _spawnPoints)
            {
                if (spawnPoint._roomID == _roomManager._roomID)
                    _spawnedGhosts.Add(spawnPoint.SpawnGhost());
            }
            FillThrowingGhostList?.Invoke(this, new EventArgs());

            //Sound
            _startingPitch = _spawnedGhosts[0].GetComponent<AudioSource>().pitch;
        }
    }
}