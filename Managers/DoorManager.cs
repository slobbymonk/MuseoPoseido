using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private RoomManager _roomManager;

    public Animator[] _animator;

    private void Start()
    {
        _roomManager= GetComponent<RoomManager>();

        _roomManager.StartedRoom += CloseDoors;
        _roomManager.EndedRoom += OpenDoors;
    }
    private void OpenDoors(object sender, EventArgs args)
    {
        if (HasDoor("OpenDoor"))
        {
            foreach (var door in _animator)
            {
                door.SetTrigger("OpenDoor");
            }
        }
    }
    private void CloseDoors(object sender, EventArgs args)
    {
        if (HasDoor("CloseDoor"))
        {
            foreach (var door in _animator)
            {
                door.SetTrigger("CloseDoor");
            }
        }
    }
    bool HasDoor(string action)
    {
        if (_animator == null)
        {
            Debug.LogError("DoorManager does not have a door animator so action " + action + " could not be performed");
            return false;
        }
        else
            return true;
    }
}
