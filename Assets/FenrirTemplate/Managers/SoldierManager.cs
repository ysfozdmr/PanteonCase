using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Fenrir.EventBehaviour;
using Fenrir.Managers;
using UnityEngine;

namespace Fenrir.Managers
{
    public class SoldierManager : EventBehaviour<SoldierManager>
    {
        public GameObject soldier;
        public Vector3 destinationPosition;
        public PathFinding pathFinding;

        public PlacementSystem placementSystem;

        public Grid grid;


       
        void Start()
        {
        }

        public void Movement(List<Vector3Int> _pathFinding, GameObject _soldier)
        {
            if (_pathFinding.Count>0 && _soldier != null)
            {
                StartCoroutine(Walk(_pathFinding, _soldier));      
            }
        }

        IEnumerator Walk(List<Vector3Int> _pathFinding, GameObject _soldier)
        {
            int tempCount = _pathFinding.Count;
            placementSystem.StartRemoving(grid.WorldToCell(_soldier.transform.position));
            placementSystem.GetSoldierMovementPlacement(grid.WorldToCell(_soldier.transform.position),
                _pathFinding[tempCount - 1]);
           
            for (int i = 0; i < tempCount; i++)
            {
                float x = 0;
                while (x <= .4)
                {
                    _soldier.transform.position =
                        Vector3.Lerp(_soldier.transform.position, _pathFinding[0], 7*Time.deltaTime);
                    x += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
              
                _soldier.transform.position = _pathFinding[0];
                yield return new WaitForEndOfFrame();

                _pathFinding.RemoveAt(0);
            }

            if ((soldier.GetComponent<SoldierBehaviour>()._state == SoldierBehaviour.State.Attack))
            {
                StartCoroutine(AttackingNum(soldier, null));
            }
            else if (soldier.GetComponent<SoldierBehaviour>()._state == SoldierBehaviour.State.Walk)
            {
                pathFinding.isPathDone = false;
                soldier.GetComponent<SoldierBehaviour>()._state = SoldierBehaviour.State.Idle;
            }
        }

        IEnumerator AttackingNum(GameObject soldier, GameObject temp)
        {
            GameObject tempObject = temp;
            if (InputManager.Instance.GetTarget() != null || tempObject != null)
            {
                if (InputManager.Instance.GetTarget() != null)
                {
                    tempObject = InputManager.Instance.GetTarget();
                }

                if (soldier != tempObject)
                {
                    soldier.GetComponent<SoldierBehaviour>().Attack(tempObject);
                }
            }
            else
            {
                Debug.Log("target null");
            }

            yield return new WaitForSeconds(1f);

            if (tempObject.TryGetComponent<SoldierBehaviour>(out var soldierBehaviour))
            {
                if (soldierBehaviour.Health > 0)
                {
                    StartCoroutine(AttackingNum(soldier, tempObject));
                    yield break;
                }
                else
                {
                    placementSystem.GetDestroyMethod(grid.WorldToCell(tempObject.transform.position));
                    yield break;
                }
            }

            if (tempObject.TryGetComponent<BuildingsBehaviour>(out var buildingsBehaviour))
            {
                if (buildingsBehaviour.Health > 0)
                {
                    StartCoroutine(AttackingNum(soldier, tempObject));
                    yield break;
                }
                else
                {
                    soldier.GetComponent<SoldierBehaviour>()._state = SoldierBehaviour.State.Idle;
                    placementSystem.GetDestroyMethod(grid.WorldToCell(tempObject.transform.position));
                    pathFinding.isPathDone = false;
                    yield break;
                }
            }
        }

        void Update()
        {
            
            if (Input.GetMouseButtonDown(1) &&  soldier.GetComponent<SoldierBehaviour>()._state == SoldierBehaviour.State.Idle)
            {
                placementSystem.RemovingStateStart();
                destinationPosition = InputManager.Instance.GetSelectedMapPosition();

                pathFinding.CheckPath(soldier,grid.WorldToCell(soldier.transform.position),
                    grid.WorldToCell(destinationPosition));
            }

            if (Input.GetMouseButtonDown(0))
            {
                soldier = InputManager.Instance.GetSoldier();
            }
        }
    }
}