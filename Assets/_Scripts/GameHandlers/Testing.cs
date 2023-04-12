using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    [SerializeField] private UnitHandler unit;
    void Start()
    {

    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.T)) {
            GridVisualManager.INSTANCE.HideAllGridVisuals();
            GridVisualManager.INSTANCE.ShowValidGridVisuals(unit.GetMoveAction().ListValidActionPositions());
        }
    }
    
}
