using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTutanhamon : MonoBehaviour
{
    private RoomManager _roomManager;

    [SerializeField] private GameObject _tut;
    [SerializeField] private Transform _tutSpawnPoint;

    private bool _skipFirst = false;

    private void Start()
    {
        _roomManager = GetComponent<RoomManager>();

        _roomManager.StartedRoom += SpawnTut;
    }
    private void SpawnTut(object sender, EventArgs args)
    {

        if (_skipFirst){

            Debug.Log("Spawned TUT");
            var newArtifact = Instantiate(_tut, _tutSpawnPoint.position, _tut.transform.rotation);
            newArtifact.transform.parent = this.transform.GetChild(0);
        }
        else
            _skipFirst = true;
    }
}
