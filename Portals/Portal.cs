using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal _connectedPortal;

    private bool _isDisabled;

    public float _teleportCooldown;
    private float _coolingDown;

    private float _portalForce = 1000f;

    private void Update()
    {
        HandleCooldown();
    }

    void Teleport(Transform teleportationTarget)
    {        
        teleportationTarget.position = _connectedPortal.transform.position + _connectedPortal.transform.up;
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<ITeleportable>() != null && !_isDisabled)
        {
            if(other.gameObject.CompareTag("Player")) AdjustOtherVelocityToOtherPortal(other.gameObject.GetComponent<Rigidbody>());
            Teleport(other.transform);
            _isDisabled = true;
            _connectedPortal._isDisabled = true;
        }
    }
    void AdjustOtherVelocityToOtherPortal(Rigidbody rb)
    {
        var cameraTransform = Camera.main.transform;
        var outTransform = _connectedPortal.transform.rotation;

        cameraTransform.rotation = new Quaternion(outTransform.x, outTransform.y, outTransform.z, 0);
    }
    void HandleCooldown()
    {
        if(_isDisabled)
        {
            if (_coolingDown > _teleportCooldown)
            {
                _coolingDown = 0;
                _isDisabled = false;
            }
            else
            {
                _coolingDown += Time.deltaTime;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.up);
    }
}
