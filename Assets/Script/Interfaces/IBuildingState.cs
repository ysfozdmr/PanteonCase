using UnityEngine;

public interface IBuildingState
{
    public void EndState();
    public void OnAction(Vector3Int gridPosition);
    public void OnActionSoldier(Vector3Int gridPosition,int selectedSoldierIndex,GameObject field);
    public void UpdateState(Vector3Int gridPosition);
    
    
}
