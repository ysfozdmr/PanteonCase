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

    public IBuildingState _buildingState;

    public GameObject field;

    [SerializeField] private PathFinding _pathFinding;

    public bool isSoldierMove;
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
        DataManager.Instance.SelectedObjectIndex = _dataBaseSo.objectsData.FindIndex(data => data.ID == id);
        gridVisualizastion.SetActive(true);
        _buildingState = new PlacementState(id, _grid, preview, _dataBaseSo, floarData, buildingsData, _objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving(Vector3Int gridPos)
    {
        StopPlacement();
        gridVisualizastion.SetActive(true);
        _buildingState = new RemovingState(_grid,_dataBaseSo, floarData, buildingsData, _objectPlacer);
        _buildingState.OnAction(gridPos);
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

    public void GetDestroyMethod(Vector3Int gridPos)
    { 
        _buildingState.DestroyObject(gridPos);
    }
    public bool CheckPlacementValiditiy(Vector3Int gridPos, int selectedObjectIndexs)
    {
        return _buildingState.CheckPlacementValiditiy(gridPos, selectedObjectIndexs);
    }
    public void PlaceSoldier(int id)
    {
        _buildingState = new PlacementState(id, _grid, preview, _dataBaseSo, floarData, buildingsData, _objectPlacer);
        Vector3 soldierPos;
        Vector3Int gridPos;

        soldierPos =inputManager.GetBarrackPosition() + Vector3.right * 2;
        gridPos = _grid.WorldToCell(soldierPos);
        Debug.Log(gridPos);
        _buildingState.OnActionSoldier(gridPos, _dataBaseSo.objectsData.FindIndex(data => data.ID == id));
        
    }
    public void GetSoldierMovementPlacement(Vector3Int firstPos,Vector3Int lastPos)
    {
        if (firstPos !=null && lastPos!=null)
        {
            _buildingState.SoldierMovementPlacement(firstPos,lastPos);     
        }
       
    }
    public override void ActorUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inputManager.GetBarrackPosition();
            inputManager.GetObjectsName();
            UIManager.Instance.RightSidePanel.GetComponent<RightSidePanel>().PanelOpen();
        }
        
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