using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScreen : MonoBehaviour
{
    public float _scrollDownSpeed;

    private void Update()
    {
        transform.position += new Vector3(0, _scrollDownSpeed * Time.deltaTime);
    }
}
