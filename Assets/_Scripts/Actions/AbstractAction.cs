using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAction : MonoBehaviour {
    
    protected UnitHandler unit;
    protected Action onActionComplete;

    protected virtual void Awake() {
        unit = GetComponent<UnitHandler>();
    }

    public abstract void PrepareAction(TilePosition position, Action onActionComplete);

    public abstract List<TilePosition> ListValidActionPositions();

    public bool IsValidActionTilePosition(TilePosition position) {
        List<TilePosition> validMoveActionList = ListValidActionPositions();
        return validMoveActionList.Contains(position);
    }

}
