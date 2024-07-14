using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System;

public class HoldingSystem : MonoBehaviour
{
    public PlayerInput _playerInput;

    public Transform _holdingPosition;

    private IHoldable _holdingItem;

    private bool _inUse; //Only be in use when playing a room

    private enum HoldingStates
    {
        None,
        TryingToHold,
        Holding,
        Dropping
    }
    private HoldingStates _holdingState;

    public float _maxRaycastDistance;
    public LayerMask _holdingLayer;

    #region Instantiate Everything - Awake/Start
    void Awake()
    {
        _playerInput = new PlayerInput();
    }
    private void Start()
    {
        LevelManager.instance.StartedRoom += ToggleActive;
        LevelManager.instance.EndedRoom += ToggleInActive;
    }
    #endregion

    #region Input - OnEnable/OnDisable
    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Action.Holding.performed += OnStartHolding;
    }
    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Action.Holding.performed -= OnStartHolding;
    }
    #endregion

    void ToggleActive(object sender, EventArgs args)
    {
        _inUse = true;
    }
    void ToggleInActive(object sender, EventArgs args)
    {
        _inUse = false;
    }

    private void OnStartHolding(InputAction.CallbackContext obj)
    {
        if (_inUse)
        {
            if (_holdingState == HoldingStates.Holding)
                _holdingState = HoldingStates.Dropping;
            else if (_holdingState == HoldingStates.None)
                _holdingState = HoldingStates.TryingToHold;
            else
                _holdingState = HoldingStates.None;
        }    
    }
    private void Update()
    {
        switch (_holdingState)
        {
            case HoldingStates.None:
                break;
            case HoldingStates.TryingToHold:
                GrabbingItem();
                break;
            case HoldingStates.Holding:
                _holdingItem.IsHeld();
                break;
            case HoldingStates.Dropping:
                DropItem();
                break;
        }
    }

    void GrabbingItem()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _maxRaycastDistance, _holdingLayer))
        {
            var targetItem = hit.transform.gameObject.GetComponent<IHoldable>();

            HandlingHoldingItem(targetItem);
        }
        else
        {
            _holdingState = HoldingStates.None;
        }
    }
    void HandlingHoldingItem(IHoldable item)
    {
        _holdingItem = item;

        _holdingItem.GetGameObject().GetComponent<Rigidbody>().isKinematic = true;
        _holdingItem.GetGameObject().GetComponent<Collider>().enabled = false;

        _holdingItem.GetGameObject().transform.parent = _holdingPosition;
        _holdingItem.GetGameObject().transform.localPosition = Vector3.zero;
        _holdingItem.GetGameObject().transform.localEulerAngles = _holdingPosition.eulerAngles;

        _holdingState = HoldingStates.Holding;
    }
    void DropItem()
    {
        //Debug.Log("Dropped");
        _holdingItem.GetGameObject().transform.parent = null;

        _holdingItem.GetGameObject().GetComponent<Rigidbody>().isKinematic = false;
        _holdingItem.GetGameObject().GetComponent<Collider>().enabled = true;

        _holdingState = HoldingStates.None;
        _holdingItem.IsDropped();
    }
}
