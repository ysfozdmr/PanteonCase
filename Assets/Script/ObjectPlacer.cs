using System.Collections;
using System.Collections.Generic;
using Fenrir.Actors;
using Fenrir.Managers;
using UnityEngine;

public class ObjectPlacer : GameActor<GameManager>
{
    public List<GameObject> placedGameObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position,int id)
    {
        GameObject building = Instantiate(prefab);
        if ( prefab.TryGetComponent<SoldierBehaviour>(out var soldierBehaviour))
        {
            soldierBehaviour.ID = id;
        }

       
        building.transform.position = position;
        placedGameObjects.Add(building);
        return placedGameObjects.Count - 1;
    }

    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex 
            || placedGameObjects[gameObjectIndex] == null)
        {
            return;
        }

        Debug.Log(placedGameObjects[gameObjectIndex].transform.position);
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
    }
}