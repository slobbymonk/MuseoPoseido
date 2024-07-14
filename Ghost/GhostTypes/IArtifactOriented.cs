using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArtifactOriented
{
    public Artifacts _targetItem { get; set; }
    public float _findItemRange { get; set; }
    public LayerMask _ignoreLayersForPlayerSearch { get; set; }
    public Transform _itemHoldingPosition { get; set; }

    void FindTarget();
    bool CheckIfNotAlreadyPossessed(GameObject item);
    void PossessItem();
}
