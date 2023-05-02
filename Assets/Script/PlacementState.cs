using System.Collections;
using System.Collections.Generic;
using Fenrir.Managers;
using Unity.VisualScripting;
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

    private int checkCounter;

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
            DataManager.Instance.SelectedObjectIndex = selectedObjectIndex;
        }
        else
            throw new System.Exception($"No object with ID {iD}");
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    private bool CheckPlacementValiditiy(Vector3Int gridPos, int selectedObjectIndexs)
    {
        GridData selectedData = database.objectsData[selectedObjectIndexs].isInteractable == true
            ? floorData
            : buildingsData;
        return selectedData.CanPlaceObject(gridPos, database.objectsData[selectedObjectIndexs].Size);
    }

    private bool SoldierCheckPlacementValiditiy(Vector3Int gridPos, int selectedObjectIndexs)
    {
        GridData selectedData = database.objectsData[selectedObjectIndexs].isInteractable == true
            ? floorData
            : buildingsData;
        return selectedData.CanPlaceObject(gridPos, database.objectsData[selectedObjectIndexs].Size, Vector3.zero);
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValiditiy = CheckPlacementValiditiy(gridPosition, selectedObjectIndex);
        if (placementValiditiy == false)
        {
            return;
        }

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab,
            grid.CellToWorld(gridPosition));

        GridData selectedData = database.objectsData[selectedObjectIndex].isInteractable == true
            ? floorData
            : buildingsData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size,
            database.objectsData[selectedObjectIndex].ID,
            index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    public void OnActionSoldier(Vector3Int gridPosition, int selectedSoldierIndex, GameObject field)
    {
        bool placementValiditiy = SoldierCheckPlacementValiditiy(gridPosition, selectedSoldierIndex);
        if (placementValiditiy == false && checkCounter<3)
        {
            if (checkCounter == 0)
            {
                Vector3 pos = objectPlacer.placedGameObjects[0].transform.position + Vector3.left;
                gridPosition = grid.WorldToCell(pos);
               
            }
            else if (checkCounter == 1)
            {
                Vector3 pos = objectPlacer.placedGameObjects[0].transform.position + Vector3.up * 2;
                gridPosition = grid.WorldToCell(pos);
                
            }
            else if (checkCounter == 2)
            {
                Vector3 pos = objectPlacer.placedGameObjects[0].transform.position + Vector3.down;
                gridPosition = grid.WorldToCell(pos);
             
            }
          
            checkCounter++;
            OnActionSoldier(gridPosition, selectedSoldierIndex, field);
            return;
            Debug.Log(checkCounter);
        }

        checkCounter = 0;
        int index = objectPlacer.PlaceObject(database.objectsData[selectedSoldierIndex].Prefab,
            grid.CellToWorld(gridPosition));

        GridData selectedData = database.objectsData[selectedSoldierIndex].isInteractable == true
            ? floorData
            : buildingsData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedSoldierIndex].Size,
            database.objectsData[selectedSoldierIndex].ID,
            index);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValiditiy = CheckPlacementValiditiy(gridPosition, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValiditiy);
    }
}