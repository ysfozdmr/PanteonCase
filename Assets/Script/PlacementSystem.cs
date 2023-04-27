using System;
using System.Collections;
using System.Collections.Generic;
using Fenrir.Actors;
using Fenrir.Managers;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid _grid;

    [SerializeField] private ObjectDataBaseSO _dataBaseSo;

    [SerializeField] private GameObject gridVisualizastion;

    private GridData floarData, buildingsData;

    private Renderer prieviewRenderer;
    
    [SerializeField] private PreviewSystem preview;

    [SerializeField] private ObjectPlacer _objectPlacer;
    
    private Vector3Int lastDetectedPos = Vector3Int.zero;

    private IBuildingState _buildingState;
    private void Start()
    {
        StopPlacement();
        floarData = new ();
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
        lastDetectedPos= Vector3Int.zero;
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
    

    private void Update()
    {
        if(_buildingState==null)
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