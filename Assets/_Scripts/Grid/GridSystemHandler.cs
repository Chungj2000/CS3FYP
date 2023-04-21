using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Script for handling GridSystem data references.
 * This includes manipulation of the stored list of GridTiles such as adding, removing, or getting.
 * Also deals with utility involved with GridSystem such as converting a world position to a TilePosition and vice versa.
 * Note gridDebugPrefab references are for testing purposes.
 */
public class GridSystemHandler : MonoBehaviour {

    public static GridSystemHandler INSTANCE {get; private set;}
    //[SerializeField] private Transform gridDebugPrefab;
    private GridSystem gridSystem;

    private void Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("GridSystemHandler instance created.");
        } else {
            Debug.LogError("More than one GridSystemHandler instance created.");
            Destroy(this);
            return;
        }

        gridSystem = new GridSystem(10,10, 2f);
        //gridSystem.CreateDebugobjects(gridDebugPrefab);
    }

    //Attaches a Unit to a GridTile of a given TilePosition.
    public void AddUnitAtTilePosition(TilePosition tilePosition, UnitHandler unit) {
        GridTile gridTile = gridSystem.GetGridTile(tilePosition);
        gridTile.AddUnit(unit);
    }

    //Removes a Unit from a GridTile of a given TilePosition.
    public void RemoveUnitAtTilePosition(TilePosition tilePosition, UnitHandler unit) {
        GridTile gridTile = gridSystem.GetGridTile(tilePosition);
        gridTile.RemoveUnit(unit);
    }

    //Sets a Unit to another TilePosition from their original position, and removes the old reference.
    public void UnitMovedTilePosition(UnitHandler unit, TilePosition fromTilePosition, TilePosition toTilePosiition) {
        RemoveUnitAtTilePosition(fromTilePosition, unit);
        AddUnitAtTilePosition(toTilePosiition, unit);
    }

    //Getters below.
    public TilePosition GetTilePosition(Vector3 worldPosition) => gridSystem.GetTilePosition(worldPosition);

    public Vector3 GetWorldPosition(TilePosition tilePosition) => gridSystem.GetWorldPosition(tilePosition);

    public bool IsValidPosition(TilePosition position) => gridSystem.IsValidPosition(position);

    public int GetHeight() => gridSystem.GetHeight();

    public int GetWidth() => gridSystem.GetWidth();

    //Identifies whether a Unit reference has been attached to a GridTile from a given TilePosition.
    public bool IsOccupiedByUnit(TilePosition position) {
        GridTile gridTile = gridSystem.GetGridTile(position);
        return gridTile.HasAUnit();
    }
    
    public List<UnitHandler> GetUnitsAtTilePosition(TilePosition tilePosition) {
        GridTile gridTile = gridSystem.GetGridTile(tilePosition);
        return gridTile.GetUnitList();
    }

    public UnitHandler GetAUnitAtTilePosition(TilePosition position) {
        GridTile gridTile = gridSystem.GetGridTile(position);
        return gridTile.GetUnit();
    }

}
