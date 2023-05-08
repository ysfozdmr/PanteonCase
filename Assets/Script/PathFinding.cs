using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public List<Vector3Int> CheckPath(GameObject _soldier, Vector3Int currentGridPos, Vector3Int destinationGridPos)
    {
        // if (_soldier.GetComponent<SoldierBehaviour>()._state == SoldierBehaviour.State.Idle)
        // {
            if (isPathDone)
            {
                isFindPath = false;
                //isPathDone = false;
                return destinationPositionsList;
            }
            else
            {
                if (!_buildingState.CheckPlacementValiditiy(destinationGridPos, 5))
                {
                    if (_buildingState.CheckPlacementValiditiy(CheckRightGrid(destinationGridPos), 5))
                    {
                        destinationGridPos = CheckRightGrid(destinationGridPos);
                    }
                    else if (_buildingState.CheckPlacementValiditiy(CheckUpGrid(destinationGridPos), 5))
                    {
                        destinationGridPos = CheckUpGrid(destinationGridPos);
                    }
                    else if (_buildingState.CheckPlacementValiditiy(CheckLeftGrid(destinationGridPos), 5))
                    {
                        destinationGridPos = CheckLeftGrid(destinationGridPos);
                    }
                    else if (_buildingState.CheckPlacementValiditiy(CheckDownGrid(destinationGridPos), 5))
                    {
                        destinationGridPos = CheckDownGrid(destinationGridPos);
                    }
                    else
                    {
                        Debug.Log("Kırdım gitmiyorum");
                        return null;
                    }
                }


                tempGridPos.x = currentGridPos.x;
                tempGridPos.y = currentGridPos.y;

                crossCheckPos.x = CompareValues(currentGridPos.x, destinationGridPos.x, true);
                crossCheckPos.y = CompareValues(currentGridPos.y, destinationGridPos.y, false);

                if (_buildingState.CheckPlacementValiditiy(crossCheckPos, 5) && !isPathDone)
                {
                    tempGridPos.x = CompareValues(currentGridPos.x, destinationGridPos.x, true);
                    tempGridPos.y = CompareValues(currentGridPos.y, destinationGridPos.y, false);

                    Debug.Log("çapraz atladım ***");
                    destinationPositionsList.Add(tempGridPos);
                    isFindPath = true;
                    
                    if (tempGridPos == destinationGridPos)
                    {
                        isPathDone = true;
                    }
                    
                    if (isFindPath)
                    {
                        isFindPath = false;
                        CheckPath(_soldier,tempGridPos, destinationGridPos);
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
                        CheckAllSides(xValueIncrease, yValueIncrease, currentGridPos, destinationGridPos,_soldier);     
                    }
                    Debug.Log("ELSE **********");
                   
                }

                return null;
            }
        // }
        // else
        // {
        //     return null;
        // }
    }


    private void CheckAllSides(bool xPosValueIncrease, bool yPosValueIncrease, Vector3Int currentPos,
        Vector3Int destinationGridPos, GameObject _soldier)
    {
        if (!isPathDone)
        {
            if (xPosValueIncrease && yPosValueIncrease)
            {
                List<int> priorityList = new List<int> { 0, 1, 2, 3 };
                StartCoroutine(SetPriority(priorityList, currentPos, destinationGridPos,_soldier));
            }
            else if (xPosValueIncrease && !yPosValueIncrease)
            {
                List<int> priorityList = new List<int> { 0, 3, 1, 2 };
                StartCoroutine(SetPriority(priorityList, currentPos, destinationGridPos,_soldier));
            }
            else if (!xPosValueIncrease && yPosValueIncrease)
            {
                List<int> priorityList = new List<int> { 2, 1, 0, 3 };
                StartCoroutine(SetPriority(priorityList, currentPos, destinationGridPos,_soldier));
            }
            else
            {
                List<int> priorityList = new List<int> { 2, 3, 0, 1 };
                StartCoroutine(SetPriority(priorityList, currentPos, destinationGridPos,_soldier));
            }
        }
    }

    IEnumerator SetPriority(List<int> list, Vector3Int currentPos, Vector3Int destinationGridPos,GameObject _soldier)
    {
        if (!isPathDone)
        {
            Debug.Log("SET prİ");
            for (int i = 0; i < 4; i++)
            {
                if (isFindPath)
                {
                    break;
                }

                switch (list[i])
                {
                    case 0:
                        if (_buildingState.CheckPlacementValiditiy(CheckRightGrid(currentPos), 5))
                        {
                            tempGridPos = CheckRightGrid(currentPos);
                            destinationPositionsList.Add(CheckRightGrid(tempGridPos));
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
                        if (_buildingState.CheckPlacementValiditiy(CheckUpGrid(currentPos), 5))
                        {
                            tempGridPos = CheckUpGrid(currentPos);
                            destinationPositionsList.Add(tempGridPos);
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
                        if (_buildingState.CheckPlacementValiditiy(CheckLeftGrid(currentPos), 5))
                        {
                            tempGridPos = CheckLeftGrid(currentPos);
                            destinationPositionsList.Add(CheckLeftGrid(tempGridPos));
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
                        if (_buildingState.CheckPlacementValiditiy(CheckDownGrid(currentPos), 5))
                        {
                            tempGridPos = CheckDownGrid(currentPos);
                            destinationPositionsList.Add(CheckDownGrid(tempGridPos));
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

            CheckPath(_soldier, tempGridPos, destinationGridPos);
        }

     
    }

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
}