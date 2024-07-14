using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableUI : MonoBehaviour
{
    private bool _isActive;
    private bool _isActiveRoom;

    public GameObject _UI;
    public GameObject _rooms;

    public bool _devMode;


    void Update()
    {
        if(Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.E))
        {
            if (_devMode)
                _devMode = false;
            else
                _devMode = true;
        }   

        if (_devMode)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (_isActive)
                {
                    _isActive = false;
                    _UI.SetActive(false);
                }
                else
                {
                    _isActive = true;
                    _UI.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_isActiveRoom)
                {
                    _isActiveRoom = false;
                    _rooms.SetActive(false);
                }
                else
                {
                    _isActiveRoom = true;
                    _rooms.SetActive(true);
                }
            }
        }
    }
}
