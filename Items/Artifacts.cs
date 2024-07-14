using Ghost;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifacts : MonoBehaviour, IHoldable, ITeleportable
{
    [Header("Durability/Breaking")]
    [SerializeField] private float _durability = 100;
    float _startingDurability;
    public float _damageMultiplier;
    public GameObject _fracturedVersion;

    [Header("Set up variables")]
    private Rigidbody _rb;
    private Vector3 _startPosition;

    [Header("Possession")]
    public float _timeBeforeFallingAfterRelease;

    [SerializeField] private GameObject _fallingParticle;


    // Custom event handler delegate with an Artifacts parameter
    public delegate void OnDestroyEventHandler(Artifacts destroyedObject);
    public event OnDestroyEventHandler WhenDestroyed;

    public LayerMask GroundLayer;

    public int _roomID;

    public enum ItemState
    {
        None,
        Possessed,
        Held
    }
    public ItemState _itemState;


    private void Start()
    {
        _startingDurability = _durability;
        _startPosition = transform.position;

        _rb = GetComponent<Rigidbody>();
    }
    public float Durability
    {
        get => _durability;
        set
        {
            _durability = value;
        }
    }

    /*private void DamageFromCollision(Collision collision)
    {
        //Take damage based on velocity
        TakeDamage(collision.relativeVelocity.magnitude * _damageMultiplier);

        //if durability is at 50% then turn colour to red
        if (_durability <= (_startingDurability / 2))
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }*/
    void Update()
    {
        if (Durability <= 0)
        {
            Instantiate(_fallingParticle, new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 5, transform.position.z), _fallingParticle.transform.rotation);
            WhenGettingDestroyed();
        }/*
        if(_itemState == ItemState.None)
            WhenReleasedFromPossession();*/
    }
    public void GetThrownedIntoTheAir()
    {
        WhenPossessorBeingSucked();
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().enabled = true;
    }
    public void TakeDamage(float damage)
    {
        Durability -= damage;
    }

    #region Possession
    public bool GetPossessionState()
    {
        if (_itemState == ItemState.Possessed)
            return true;
        else
            return false;
    }
    public void WhenPossessed(Transform itemHoldingPosition)
    {
        _itemState = ItemState.Possessed;

        transform.parent = itemHoldingPosition;
        transform.localPosition = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }
    public void WhenPossessorBeingSucked()
    {
        _itemState = ItemState.None;
        transform.parent = null;
    }
    public void WhenReleasedFromPossession()
    {
        if (_itemState == ItemState.None)
        {
            GetComponent<Collider>().enabled = true;
            Invoke("ResetPossession", _timeBeforeFallingAfterRelease);
        }
    }
    private void ResetPossession()
    {
        if (_itemState == ItemState.None)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    #endregion
    private void OnCollisionEnter(Collision collision)
    {
        if ((GroundLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            //DamageFromCollision(collision);
            Durability = 0;
        }
    }

    void WhenGettingDestroyed()
    {
        WhenDestroyed?.Invoke(this.GetComponent<Artifacts>());
        Instantiate(_fracturedVersion, transform.position, transform.rotation);
        // AudioManager.instance.Play("FractureArtifact"); DO NOT ENABLE UNLESS YOU WANT A TSUNAMI OF ARTIFACTS

        Destroy(gameObject);
    }
    public void PickedUp()
    {
        _itemState = ItemState.Held;
    }

    public void IsHeld()
    {
        _itemState = ItemState.Held;
    }

    public void IsDropped()
    {
        _itemState = ItemState.None;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}
