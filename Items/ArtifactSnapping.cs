using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactSnapping : MonoBehaviour
{
    public Transform _snappingPosition;

    [SerializeField] private bool _skipFirst =false; //For some reason it warps when I rotate it on start

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Artifacts>() != null && _skipFirst)
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.transform.position = _snappingPosition.position;
            other.transform.rotation = _snappingPosition.rotation;

            AudioManager.instance.Play("ArtifactPlacing");
        }
        _skipFirst = true;
    }
}
