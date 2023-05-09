using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fenrir.Managers;

public class PathFinding : MonoBehaviour
{
    public List<Vector3Int> destinationPositionsList = new List<Vector3Int>();

    private Vector3Int tempGridPos;
    private Vector3Int crossCheckPos;

    public bool xValueIncrease;
    public bool yValueIncrease;
    
    private bool isFindPath;
    public bool isPathDone;

    [SerializeField] private PlacementSystem _buildingState;

    #region CheckPath
    
    public List<Vector3Int> CheckPath(GameObject _soldier, Vector3Int currentGridPos, Vector3Int destinationGridPos)
    {
        tempGridPos.x = currentGridPos.x;
        tempGridPos.y = currentGridPos.y;
      
        if (isPathDone)
        {
            isFindPath = false;
            _soldier.GetComponent<SoldierBehaviour>()._pathFinding.AddRange(destinationPositionsList);

            destinationPositionsList.Clear();

            if (InputManager.Instance.GetTarget() != null)
            {
                _soldier.GetComponent<SoldierBehaviour>()._state = SoldierBehaviour.State.Attack;
            }
            else
            {
                _soldier.GetComponent<SoldierBehaviour>()._state = SoldierBehaviour.State.Walk;
            }
            
            SoldierManager.Instance.Movement(_soldier.GetComponent<SoldierBehaviour>()._pathFinding, _soldier);

            return destinationPositionsList;
        }
        else
        {
           
            if (!_buildingState.CheckPlacementValiditiySystem(destinationGridPos, 5))
            {
                if (_buildingState.CheckPlacementValiditiySystem(CheckRightGrid(destinationGridPos) + Vector3Int.right, 5))
                {
                    destinationGridPos = CheckRightGrid(destinationGridPos) + Vector3Int.right;
                }
                else if (_buildingState.CheckPlacementValiditiySystem(CheckUpGrid(destinationGridPos) + Vector3Int.up, 5))
                {
                    destinationGridPos = CheckUpGrid(destinationGridPos) + Vector3Int.up;
                }
                else if (_buildingState.CheckPlacementValiditiySystem(CheckLeftGrid(destinationGridPos), 5))
                {
                    destinationGridPos = CheckLeftGrid(destinationGridPos);
                
                }
                else if (_buildingState.CheckPlacementValiditiySystem(CheckDownGrid(destinationGridPos) + Vector3Int.down, 5))
                {
                    destinationGridPos = CheckDownGrid(destinationGridPos) + Vector3Int.down;
                }
                else
                {
                    return null;
                }
            } 

            crossCheckPos.x = CompareValues(currentGridPos.x, destinationGridPos.x, true);
            crossCheckPos.y = CompareValues(currentGridPos.y, destinationGridPos.y, false);

            if (_buildingState.CheckPlacementValiditiySystem(crossCheckPos, 5) && !isPathDone)
            {
                tempGridPos.x = CompareValues(currentGridPos.x, destinationGridPos.x, true);
                tempGridPos.y = CompareValues(currentGridPos.y, destinationGridPos.y, false);
                
                destinationPositionsList.Add(tempGridPos);
                isFindPath = true;

                if (tempGridPos == destinationGridPos)
                {
                    isPathDone = true;
                }

                if (isFindPath)
                {
                    isFindPath = false;
                    CheckPath(_soldier, tempGridPos, destinationGridPos);
                    return null;
                }
            }
            else
            {
                if (tempGridPos == destinationGridPos)
                {
                    isPathDone = true;
                }
                else
                {
                    isFindPath = false;
                    CheckAllSides(xValueIncrease, yValueIncrease, tempGridPos, destinationGridPos, _soldier);
                }
            }

            return null;
        }
    
    }
    
    #endregion
    
    
    #region CheckSides
    
    private void CheckAllSides(bool xPosValueIncrease, bool yPosValueIncrease, Vector3Int currentPos,
        Vector3Int destinationGridPos, GameObject _soldier)
    {
        if (_soldier.GetComponent<SoldierBehaviour>()._state == SoldierBehaviour.State.Idle)
        {
            if (xPosValueIncrease && yPosValueIncrease)
            {
                List<int> priorityList = new List<int> { 0, 1, 2, 3 };
                StartCoroutine(SetPriority(priorityList, currentPos, destinationGridPos, _soldier));
            }
            else if (xPosValueIncrease && !yPosValueIncrease)
            {
                List<int> priorityList = new List<int> { 0, 3, 1, 2 };
                StartCoroutine(SetPriority(priorityList, currentPos, destinationGridPos, _soldier));
            }
            else if (!xPosValueIncrease && yPosValueIncrease)
            {
                List<int> priorityList = new List<int> { 1, 0, 2, 3 };
                StartCoroutine(SetPriority(priorityList, currentPos, destinationGridPos, _soldier));
            }
            else
            {
                List<int> priorityList = new List<int> { 3, 0, 2, 1 };
                StartCoroutine(SetPriority(priorityList, currentPos, destinationGridPos, _soldier));
            }
        }
    }
    
    #endregion

    #region SetPriority

    IEnumerator SetPriority(List<int> list, Vector3Int currentPos, Vector3Int destinationGridPos, GameObject _soldier)
    {
        Vector3Int newalan = new Vector3Int();
        if (_soldier.GetComponent<SoldierBehaviour>()._state == SoldierBehaviour.State.Idle)
        {
            for (int i = 0; i < 4; i++)
            {
                if (isFindPath)
                {
                    break;
                }
                switch (list[i])
                {
                    case 0:
                      
                        if (_buildingState.CheckPlacementValiditiySystem(CheckRightGrid(tempGridPos), 5))
                        {
                            newalan = CheckRightGrid(currentPos);
                            destinationPositionsList.Add(CheckRightGrid(newalan));
                            isFindPath = true;
                            yield return new WaitForEndOfFrame();
                            yield return new WaitForEndOfFrame();
                            yield return new WaitForEndOfFrame();
                            if (tempGridPos == destinationGridPos)
                            {
                                isPathDone = true;
                            }
                        }

                        break;
                    case 1:
                        if (_buildingState.CheckPlacementValiditiySystem(CheckUpGrid(tempGridPos), 5))
                        {
                            newalan = CheckUpGrid(currentPos);
                            destinationPositionsList.Add(newalan);
                            isFindPath = true;
                            yield return new WaitForEndOfFrame();
                            yield return new WaitForEndOfFrame();
                            yield return new WaitForEndOfFrame();
                            if (tempGridPos == destinationGridPos)
                            {
                                isPathDone = true;
                            }
                        }

                        break;
                    case 2:
                        if (_buildingState.CheckPlacementValiditiySystem(CheckLeftGrid(tempGridPos), 5))
                        {
                            newalan = CheckLeftGrid(currentPos);
                            destinationPositionsList.Add(CheckLeftGrid(newalan));
                            isFindPath = true;
                            yield return new WaitForEndOfFrame();
                            yield return new WaitForEndOfFrame();
                            yield return new WaitForEndOfFrame();
                            if (tempGridPos == destinationGridPos)
                            {
                                isPathDone = true;
                            }
                        }

                        break;
                    case 3:
                        if (_buildingState.CheckPlacementValiditiySystem(CheckDownGrid(tempGridPos), 5))
                        {
                            newalan = CheckDownGrid(currentPos);
                            destinationPositionsList.Add(CheckDownGrid(newalan));
                            isFindPath = true;

                            yield return new WaitForEndOfFrame();
                            yield return new WaitForEndOfFrame();
                            yield return new WaitForEndOfFrame();

                            if (tempGridPos == destinationGridPos)
                            {
                                isPathDone = true;
                            }
                        }

                        break;
                }
            }

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            CheckPath(_soldier, newalan, destinationGridPos);
            yield break;
        }
    }

    #endregion
    
    #region CompareValues

    int CompareValues(int currentValue, int destinationValue, bool isXValue)
    {
        if (destinationValue > currentValue)
        {
            currentValue += 1;
            if (isXValue)
            {
                xValueIncrease = true;
            }
            else
            {
                yValueIncrease = true;
            }
        }

        else if (destinationValue < currentValue)
        {
            currentValue -= 1;

            if (isXValue)
            {
                xValueIncrease = false;
            }
            else
            {
                yValueIncrease = false;
            }
        }

        return currentValue;
    }
    
    #endregion

    #region CheckMethods
    
    Vector3Int CheckRightGrid(Vector3Int currentGridPos)
    {
        Vector3Int tempVector = new Vector3Int();
        tempVector.x = currentGridPos.x + 1;
        tempVector.y = currentGridPos.y;
        tempVector.z = 0;
        return tempVector;
    }

    Vector3Int CheckLeftGrid(Vector3Int currentGridPos)
    {
        Vector3Int tempVector = new Vector3Int();
        tempVector.x = currentGridPos.x - 1;
        tempVector.y = currentGridPos.y;
        tempVector.z = 0;
        return tempVector;
    }

    Vector3Int CheckUpGrid(Vector3Int currentGridPos)
    {
        Vector3Int tempVector = new Vector3Int();
        tempVector.x = currentGridPos.x;
        tempVector.y = currentGridPos.y + 1;
        tempVector.z = 0;
        return tempVector;
    }

    Vector3Int CheckDownGrid(Vector3Int currentGridPos)
    {
        Vector3Int tempVector = new Vector3Int();
        tempVector.x = currentGridPos.x;
        tempVector.y = currentGridPos.y - 1;
        tempVector.z = 0;
        return tempVector;
    }
    #endregion
}