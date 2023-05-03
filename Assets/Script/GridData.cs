using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class GridData
{
    private Dictionary<Vector3, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition,
        Vector2Int objectSize,
        int ID,
        int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains this cell positions{pos}");
            }

            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, y, 0));
            }
        }

        return returnVal;
    }

    public bool CanPlaceObject(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        //Debug.Log(objectSize + " object size " + positionToOccupy[0] +" !!!");
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }

        return true;
    }


    public bool CanPlaceObject(Vector3Int gridPosition, Vector2Int objectSize, int soldierCount)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

        if (positionToOccupy[0].x > 3 || positionToOccupy[0].x < -4
                                      || positionToOccupy[0].y > 3 || positionToOccupy[0].y < -7)
        {
            return false;
        }

        if (soldierCount < 6)
        {
            foreach (var pos in positionToOccupy)
            {
                if (placedObjects.ContainsKey(pos))
                {
                    return false;
                }
            }
        }

        return true;
    }
}


public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}