using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISuckable
{
    void IsBeingSucked();
    void ResetSuck();
    GameObject GetGameObject();
    void Capture();
}
