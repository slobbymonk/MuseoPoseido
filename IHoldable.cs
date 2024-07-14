using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoldable
{
    void PickedUp();
    void IsHeld();
    void IsDropped();
    GameObject GetGameObject();
}
