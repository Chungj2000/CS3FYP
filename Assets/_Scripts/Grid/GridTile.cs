using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile {
    
    private GridSystem gridSystem;
    private TilePosition tilePosition;
    private List<UnitHandler> units;

    public GridTile(GridSystem gs, TilePosition tp) {
        gridSystem = gs;
        tilePosition = tp;
        units = new List<UnitHandler>();
    }

    public override string ToString()
    {
        string buildUnitString = "";
        foreach(UnitHandler unit in units) {
            buildUnitString += unit + "\n";
        }
        return tilePosition.ToString() + "\n" + buildUnitString;
    }

    public void AddUnit(UnitHandler unit) {
        units.Add(unit);
    }

    public void RemoveUnit(UnitHandler unit) {
        units.Remove(unit);
    }

    public List<UnitHandler> GetUnitList() {
        return units;
    }

    public bool HasAUnit() {
        return units.Count > 0;
    }

    public UnitHandler GetUnit() {
        if(HasAUnit()) {
            //Debug.Log("Unit found.");
            return units[0];
        } else {
            //Debug.Log("No Units found.");
            return null;
        }
    }

}
