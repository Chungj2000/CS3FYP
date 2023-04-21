using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*
 * An abstract class containing base functions needed for actions.
 * Includes validation, and event functins, and keeps track of what unit is performing the action.
 */
public abstract class AbstractAction : MonoBehaviour {
    
    protected UnitHandler unit;
    protected Action onActionComplete;

    protected float stoppingDistance = .05f;

    protected PhotonView view;

    protected virtual void Awake() {
        unit = GetComponent<UnitHandler>();
        view = GetComponent<PhotonView>();
    }

    public abstract void PrepareAction(TilePosition position, Action onActionComplete);

    public abstract List<TilePosition> ListValidActionPositions();

    public bool IsValidActionTilePosition(TilePosition position) {
        List<TilePosition> validMoveActionList = ListValidActionPositions();
        return validMoveActionList.Contains(position);
    }

}
