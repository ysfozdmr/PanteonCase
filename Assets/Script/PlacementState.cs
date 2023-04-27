using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectDataBaseSO database;
    GridData floorData;
    GridData buildingsData;
    ObjectPlacer objectPlacer;
    public PlacementState(int iD,
        Grid grid,
        PreviewSystem previewSystem,
        ObjectDataBaseSO database,
        GridData floorData,
        GridData buildingsData,
        ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.buildingsData = buildingsData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                database.objectsData[selectedObjectIndex].Prefab,
                database.objectsData[selectedObjectIndex].Size);
        }
        else
            throw new System.Exception($"No object with ID {iD}");
        
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }
    private bool CheckPlacementValiditiy(Vector3Int gridPos, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? 
            floorData : buildingsData;
        return selectedData.CanPlaceObject(gridPos, database.objectsData[selectedObjectIndex].Size);
    }
    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValiditiy = CheckPlacementValiditiy(gridPosition, selectedObjectIndex);
        if (placementValiditiy == false)
        {
            return;
        }

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));
        
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? 
            floorData : buildingsData;
        selectedData.AddObjectAt(gridPosition,database.objectsData[selectedObjectIndex].Size,
            database.objectsData[selectedObjectIndex].ID,
            index);
        
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition),false);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValiditiy = CheckPlacementValiditiy(gridPosition, selectedObjectIndex);
        
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition),placementValiditiy);
    }
}
