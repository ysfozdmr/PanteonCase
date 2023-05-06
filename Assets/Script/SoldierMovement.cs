using System.Collections;
using System.Collections.Generic;
using Fenrir.Managers;
using UnityEngine;

public class SoldierMovement : MonoBehaviour
{
    public GameObject soldier;
    public Vector3 destinationPosition;
    public PathFinding pathFinding;

    public Grid grid;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            destinationPosition=InputManager.Instance.GetSelectedMapPosition();
            pathFinding.CheckPath(grid.WorldToCell(soldier.transform.position), grid.WorldToCell(destinationPosition));
            soldier.GetComponent<soldier>()._pathFinding = pathFinding.destinationPositionsList;
            soldier.GetComponent<soldier>().Movement();
        }        
        if (Input.GetMouseButtonDown(0))
        {
           soldier=InputManager.Instance.GetSoldier();
        }
    }
}
