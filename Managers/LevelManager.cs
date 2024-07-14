using Ghost;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public int _currentRoom;

    public GameObject[] _roomArtifacts;

    public event EventHandler StartedRoom;
    public event EventHandler EndedRoom;

    public AudioSource _activeRoomAmbientSound;

    private void Awake()
    {
        instance = this;
    }

    public void DisableArtifactList(int i)
    {
        _roomArtifacts[i].SetActive(false);
    }
    public void DisableAllArtifactList()
    {
        foreach (var item in _roomArtifacts)
        {
            item.SetActive(false);
        }
    }
    public void EnableAllArtifactList()
    {
        foreach (var item in _roomArtifacts)
        {
            item.SetActive(true);
        }
    }
    private void Update()
    {
        /*if(Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        Debug.Log("Artifacts: " + GameObject.FindObjectsOfType<Artifacts>().Length);
        Debug.Log("Ghosts: " + GameObject.FindObjectsOfType<Ghost.Ghost>().Length);*/
    }
    public void StartRoom()
    {
        StartedRoom?.Invoke(this, new EventArgs());
    }
    public void EndRoom()
    {
        EnableAllArtifactList();
        EndedRoom?.Invoke(this, new EventArgs());
    }
    public void SetActiveRoomAmbientSound(AudioClip audioClip)
    {
        _activeRoomAmbientSound.clip = audioClip;
        _activeRoomAmbientSound.Play();
    }
    public void SetActiveRoom(int i)
    {
        DisableAllArtifactList();
        _currentRoom = i;
        _roomArtifacts[_currentRoom].gameObject.SetActive(true);
    }
    public int GeActivetRoom()
    {
        return _currentRoom;
    }
}
