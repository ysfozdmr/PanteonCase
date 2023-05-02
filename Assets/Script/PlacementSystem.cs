using System;
using System.Collections;
using System.Collections.Generic;
using Fenrir.Actors;
using Fenrir.Managers;
using UnityEngine;

public class PlacementSystem : GameActor<GameManager>
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid _grid;

    [SerializeField] private ObjectDataBaseSO _dataBaseSo;

    [SerializeField] private GameObject gridVisualizastion;

    public GridData floarData, buildingsData;

    private Renderer prieviewRenderer;

    [SerializeField] private PreviewSystem preview;

    [SerializeField] private ObjectPlacer _objectPlacer;

    private Vector3Int lastDetectedPos = Vector3Int.zero;

    private IBuildingState _buildingState;

    public GameObject field;

    public override void ActorStart()
    {
        StopPlacement();
        floarData = new();
        buildingsData = new();
    }

    private void StopPlacement()
    {
        if (_buildingState == null)
        {
            return;
        }

        gridVisualizastion.SetActive(false);
        _buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPos = Vector3Int.zero;
        _buildingState = null;
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        gridVisualizastion.SetActive(true);
        _buildingState = new PlacementState(id, _grid, preview, _dataBaseSo, floarData, buildingsData, _objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = _grid.WorldToCell(mousePos);

        _buildingState.OnAction(gridPos);
    }

    public void PlaceSoldier(int selectedSoldierIndex)
    {
        Vector3 soldierPos;
        Vector3Int gridPos;

        soldierPos = _objectPlacer.placedGameObjects[0].transform.position + Vector3.right * 2;
        gridPos = _grid.WorldToCell(soldierPos);
        _buildingState.OnActionSoldier(gridPos, selectedSoldierIndex, field);
        
    }

    public override void ActorUpdate()
    {
        if (_buildingState == null)
            return;
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = _grid.WorldToCell(mousePos);

        if (lastDetectedPos != gridPos)
        {
            _buildingState.UpdateState(gridPos);
            lastDetectedPos = gridPos;
        }
    }
}