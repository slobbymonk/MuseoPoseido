using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private bool _hasBeenCompleted;

    public event EventHandler StartedRoom;
    public event EventHandler EndedRoom;

    private bool _roomStarted;


    public GameObject _scoreCard;

    public AudioClip _ambientSound;

    //For data reasons
    public List<Transform> _allArtifacts = new List<Transform>();

    //Needed for the saving data to JSON
    private int _artifactAmount;

    public int _roomID;

    private bool _ghostsCleared;

    private bool _playerInStartingRange;

    private void Awake()
    {
        FindAllArtifactsBelongingToTheRoom();
        _scoreCard.SetActive(false);
    }
    private void Start()
    {
        SetArtifactAmount();
    }
    private void Update()
    {
        if (_playerInStartingRange)
        {
            if (Input.GetKeyDown(KeyCode.E)) Restart();
        }
    }

    #region Artifact Amount for JSON Data Saving
    private void SetArtifactAmount()
    {
        _artifactAmount = _allArtifacts.Count;
    }

    public int GetArtifactAmount()
    {
        return _artifactAmount;
    }
    #endregion

    private void FindAllArtifactsBelongingToTheRoom()
    {
        _allArtifacts.Clear();
        var arrayOfArtifacts = FindObjectsOfType<Artifacts>();
        foreach (var artifact in arrayOfArtifacts)
        {
            if (artifact._roomID == _roomID)
            {
                _allArtifacts.Add(artifact.transform);
            }
        }
    }

    void Restart()
    {
        //So far has same functionality as StartRoom, but might differ so I'm keeping it a seperate function
        FindAllArtifactsBelongingToTheRoom();
        SetArtifactAmount();
        StartRoom();
    }
    public void GhostsClearedOut()
    {
        _ghostsCleared = true;
    }
    public void ClearedRoom()
    {
        if(_roomStarted) //Make sure room was actually started
        {
            LevelManager.instance.EndRoom();
            DestroyArtifacts(); //Remove all unbroken & broken artifacts

            AudioManager.instance.Play("FinishRoom");

            _scoreCard.SetActive(true);
            _roomStarted = false;
            _hasBeenCompleted = true;
            EndedRoom?.Invoke(this, new EventArgs());

            //TODO: Add functionality so that it also checks if the room has been completed before & doesn't redo the score if it's higher than the new one
        }
    }

    private void DestroyArtifacts()
    {
        FindAllArtifactsBelongingToTheRoom(); //Find the remaining non broken artifacts
        foreach (var artifact in _allArtifacts)
        {
            Destroy(artifact.gameObject);
        }
        foreach (var brokenArtifact in GameObject.FindGameObjectsWithTag("BrokenArtifact"))//Find the broken artifacts
        {
            Destroy(brokenArtifact);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //StartRoom
        if (other.gameObject.CompareTag("Player") && !_roomStarted)
        {
            if(_hasBeenCompleted)
                _playerInStartingRange = true;
            else StartRoom();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && _playerInStartingRange)
            _playerInStartingRange = false;
    }

    private void StartRoom()
    {
        _roomStarted = true;
        StartedRoom?.Invoke(this, new EventArgs());

        AudioManager.instance.Play("StartRoom");

        _scoreCard.SetActive(false);

        LevelManager.instance.SetActiveRoom(_roomID);
        LevelManager.instance.StartRoom();
        LevelManager.instance.SetActiveRoomAmbientSound(_ambientSound);
    }
}
