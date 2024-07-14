using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPiecesDespawning : MonoBehaviour
{
    public float _despawnTime;

    private Vector3 _scale;

    private float _despawnedTime;

    private void Awake()
    {
        _scale = transform.localScale;
    }

    void Update()
    {
        _despawnedTime += Time.deltaTime;
        float endingScale = transform.localScale.x * .7f;
        transform.localScale = Vector3.Lerp(_scale, new Vector3(endingScale, endingScale, endingScale), _despawnedTime / _despawnTime);
        if(_despawnedTime >= _despawnTime)
            Destroy(gameObject);
    }
}
