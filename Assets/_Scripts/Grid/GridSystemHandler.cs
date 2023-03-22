using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemHandler : MonoBehaviour {

    public static GridSystemHandler INSTANCE {get; private set;}
    [SerializeField] private Transform gridDebugPrefab;
    private GridSystem gridSystem;

    private void Awake()
    {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("GridSystemHandler instance created.");
        } else {
            Debug.LogError("More than one GridSystemHandler instance created.");
            Destroy(gameObject);
            return;
        }

        gridSystem = new GridSystem(10,10, 2f);
        gridSystem.CreateDebugobjects(gridDebugPrefab);
    }

    public void AddUnitAtTilePosition(TilePosition tilePosition, UnitHandler unit) {
        GridTile gridTile = gridSystem.GetGridTile(tilePosition);
        gridTile.AddUnit(unit);
    }

    public void RemoveUnitAtGridPosition(TilePosition tilePosition, UnitHandler unit) {
        GridTile gridTile = gridSystem.GetGridTile(tilePosition);
        gridTile.RemoveUnit(unit);
    }

    public void UnitMovedTilePosition(UnitHandler unit, TilePosition fromTilePosition, TilePosition toTilePosiition) {
        RemoveUnitAtGridPosition(fromTilePosition, unit);
        AddUnitAtTilePosition(toTilePosiition, unit);
    }

    public TilePosition GetTilePosition(Vector3 worldPosition) => gridSystem.GetTilePosition(worldPosition);

    public Vector3 GetWorldPosition(TilePosition tilePosition) => gridSystem.GetWorldPosition(tilePosition);

    public bool IsValidPosition(TilePosition position) => gridSystem.IsValidPosition(position);

    public int GetHeight() => gridSystem.GetHeight();

    public int GetWidth() => gridSystem.GetWidth();

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
