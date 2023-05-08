using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public List<Vector3Int> destinationPositionsList = new List<Vector3Int>();

    private Vector3Int tempGridPos;
    private Vector3Int tempVector3IntPos;

    public bool xValueIncrease;
    public bool yValueIncrease;

    private bool isFindPath;

    private int sw;
    [SerializeField] private PlacementSystem _buildingState;

    public List<Vector3Int> CheckPath(Vector3Int currentGridPos, Vector3Int destinationGridPos)
    {
        
        tempGridPos.x = currentGridPos.x;
        tempGridPos.y = currentGridPos.y;

        tempGridPos.x = CompareValues(currentGridPos.x, destinationGridPos.x, true);
        tempGridPos.y = CompareValues(currentGridPos.y, destinationGridPos.y, false);

        if (_buildingState.CheckPlacementValiditiy(tempGridPos, 2))
        {
            destinationPositionsList.Add(tempGridPos);
        }
        else
        {
            CheckAllSides(xValueIncrease, yValueIncrease, currentGridPos);
        }

        if (tempGridPos == destinationGridPos)
        {
            return destinationPositionsList;
        }
        else
        {
            CheckPath(tempGridPos, destinationGridPos);
        }

        return null;
    }

    private void CheckAllSides(bool xPosValueIncrease, bool yPosValueIncrease, Vector3Int currentPos)
    {
        if (xPosValueIncrease && yPosValueIncrease)
        {
            List<int> priorityList = new List<int> { 0,1,2,3 };
            SetPriority(priorityList,currentPos);
            
        }
        else if (xPosValueIncrease && !yPosValueIncrease)
        {
            List<int> priorityList = new List<int> { 0,2,1,3 };
            SetPriority(priorityList,currentPos);
        }
        else if (!xPosValueIncrease && yPosValueIncrease)
        {
            List<int> priorityList = new List<int> { 2,1,3,0 };
            SetPriority(priorityList,currentPos);
        }
        else
        {
            List<int> priorityList = new List<int> { 2,3,1,0 };
            SetPriority(priorityList,currentPos);
        }
    }

    void SetPriority(List<int> list, Vector3Int currentPos)
    {
        for (int i = 0; i < 4; i++)
        {
            if (isFindPath)
            {
                isFindPath = false;
                break;
            }
            switch (list[i])
            {
                case 0:
                    if (_buildingState.CheckPlacementValiditiy(CheckRightGrid(currentPos), 2))
                    {
                        destinationPositionsList.Add(CheckRightGrid(currentPos));
                        isFindPath = true;
                    }

                    break;
                case 1:
                    if (_buildingState.CheckPlacementValiditiy(CheckUpGrid(currentPos), 2))
                    {
                        destinationPositionsList.Add(CheckUpGrid(currentPos));
                        isFindPath = true;

                    }

                    break;
                case 2:
                    if (_buildingState.CheckPlacementValiditiy(CheckLeftGrid(currentPos), 2))
                    {
                        destinationPositionsList.Add(CheckLeftGrid(currentPos));
                        isFindPath = true;

                    }

                    break;
                case 3:
                    if (_buildingState.CheckPlacementValiditiy(CheckDownGrid(currentPos), 2))
                    {
                        destinationPositionsList.Add(CheckDownGrid(currentPos));
                        isFindPath = true;

                    }

                    break;
            }
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