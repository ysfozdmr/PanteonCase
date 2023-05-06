using System;
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

    public List<Vector3Int> placedSoldier = new List<Vector3Int>();

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
        if (selectedObjectIndex > -1 && selectedObjectIndex < 2)
        {
            previewSystem.StartShowingPlacementPreview(
                database.objectsData[selectedObjectIndex].Prefab,
                database.objectsData[selectedObjectIndex].Size);
        }
        else if (selectedObjectIndex > -1)
        {
            //DataManager.Instance.SelectedObjectIndex = selectedObjectIndex;
        }
        else
            throw new System.Exception($"No object with ID {iD}");
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public bool CheckPlacementValiditiy(Vector3Int gridPos, int selectedObjectIndexs)
    {
        GridData selectedData = database.objectsData[selectedObjectIndexs].isInteractable == true
            ? floorData
            : buildingsData;
        return selectedData.CanPlaceObject(gridPos, database.objectsData[selectedObjectIndexs].Size);
    }

    private bool SoldierCheckPlacementValiditiy(Vector3Int gridPos, int selectedObjectIndexs, int soldierCount)
    {
        GridData selectedData = database.objectsData[selectedObjectIndexs].isInteractable == true
            ? floorData
            : buildingsData;
        return selectedData.CanPlaceObject(gridPos, database.objectsData[selectedObjectIndexs].Size, soldierCount);
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

    public void OnActionSoldier(Vector3Int gridPosition, int selectedSoldierIndex)
    {
        Vector3 pos = InputManager.Instance.GetBarrackPosition();
        bool placementValiditiy =
            SoldierCheckPlacementValiditiy(gridPosition, selectedSoldierIndex, placedSoldier.Count);
        if (placementValiditiy == false)
        {
            CheckCellPosition(pos, gridPosition, selectedSoldierIndex);
        }
        else
        {
            PlaceSoldier(gridPosition, selectedSoldierIndex);
        }
    }


    private void CheckCellPosition(Vector3 pos, Vector3Int gridPosition, int selectedSoldierIndex)
    {
        if (checkCounter < 6)
        {
            switch (checkCounter)
            {
                case 0:
                    gridPosition = ArrangeCellPosition(pos, Vector3.left, gridPosition);

                    break;
                case 1:
                    gridPosition = ArrangeCellPosition(pos, Vector3.up * 2, gridPosition);
                    break;
                case 2:
                    gridPosition = ArrangeCellPosition(pos, Vector3.down, gridPosition);
                    break;
                case 3:
                    gridPosition = ArrangeCellPosition(pos, Vector3.right + Vector3.up, gridPosition);
                    break;
                case 4:
                    gridPosition = ArrangeCellPosition(pos, Vector3.down + Vector3.right, gridPosition);
                    break;
                case 5:
                    gridPosition = ArrangeCellPosition(pos, Vector3.up + Vector3.left, gridPosition);
                    break;
            }

            checkCounter++;
            OnActionSoldier(gridPosition, selectedSoldierIndex);
            return;
        }
    }

    private Vector3Int ArrangeCellPosition(Vector3 position, Vector3 direction, Vector3Int gridPosition)
    {
        position += direction;
        gridPosition = grid.WorldToCell(position);
        return gridPosition;
    }

    private void PlaceSoldier(Vector3Int gridPosition, int selectedSoldierIndex)
    {
        checkCounter = 0;
        int index = objectPlacer.PlaceObject(database.objectsData[selectedSoldierIndex].Prefab,
            grid.CellToWorld(gridPosition));

        GridData selectedData = database.objectsData[selectedSoldierIndex].isInteractable == true
            ? floorData
            : buildingsData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedSoldierIndex].Size,
            database.objectsData[selectedSoldierIndex].ID,
            index);
        placedSoldier.Add(gridPosition);
    }

    public void UpdateState(Vector3Int gridPosition,bool isSoldierMove)
    {
        if (isSoldierMove)
        {
            bool placementValiditiy = CheckPlacementValiditiy(gridPosition, selectedObjectIndex);

            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValiditiy);
        }
    }
}