using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    ObjectDataBaseSO database;
    Grid grid;
    GridData floorData;
     GridData buildings;
    ObjectPlacer objectPlacer;

    public RemovingState(Grid grid,
        ObjectDataBaseSO database,
        GridData floorData,
        GridData buildings,
        ObjectPlacer objectPlacer)
    {
        this.database = database;
        this.grid = grid;
        this.floorData = floorData;
        this.buildings = buildings;
        this.objectPlacer = objectPlacer;

    }

    public void EndState()
    {
        Debug.Log("end");
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if(buildings.CanPlaceObject(gridPosition,Vector2Int.one) == false)
        {
            selectedData = buildings;
        }
        else if(floorData.CanPlaceObject(gridPosition, Vector2Int.one) == false)
        {
            selectedData = floorData;
        }

        if(selectedData == null)
        {
            //sound
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex == -1)
                return;
            selectedData.RemoveObjectAt(gridPosition);
        }
    }

    public void DestroyObject(Vector3Int gridPos)
    {
        GridData selectedData = floorData;
        int index = selectedData.GetRepresentationIndex(gridPos);
        objectPlacer.RemoveObjectAt(index);
        selectedData.RemoveObjectAt(gridPos);
    }
    public void OnActionSoldier(Vector3Int gridPosition, int selectedSoldierIndex)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState(Vector3Int gridPosition)
    {
    }

    public bool CheckPlacementValiditiy(Vector3Int gridPos, int selectedObjectIndexs)
    {
        GridData selectedData = database.objectsData[selectedObjectIndexs].isInteractable == true
            ? floorData
            : buildings;
        return selectedData.CanPlaceObject(gridPos, database.objectsData[selectedObjectIndexs].Size);
    }

    public void SoldierMovementPlacement(Vector3Int firstPos, Vector3Int lastPos)
    {
        GridData selectedData = database.objectsData[2].isInteractable == true
            ? floorData
            : buildings;
        selectedData.AddObjectAt(lastPos,new Vector2Int(1,1),10,2);
    }
}
