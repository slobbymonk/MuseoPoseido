using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPillar : MonoBehaviour
{
    private bool _skipFirst = false; //For some reason it warps when I rotate it on start

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Artifacts>() != null && _skipFirst)
        {
            other.GetComponent<Collider>().enabled = true;
            other.GetComponent<Rigidbody>().isKinematic = false;
        }
        _skipFirst = true;
    }
}
