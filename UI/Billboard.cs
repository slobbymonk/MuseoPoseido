using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        transform.LookAt(_player.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, transform.eulerAngles.z);
    }
}
