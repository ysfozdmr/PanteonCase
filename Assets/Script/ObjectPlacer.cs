using System.Collections;
using System.Collections.Generic;
using Fenrir.Actors;
using Fenrir.Managers;
using UnityEngine;

public class ObjectPlacer : GameActor<GameManager>
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject building = Instantiate(prefab);
        building.transform.position = position;
        placedGameObjects.Add(building);
        return placedGameObjects.Count - 1;
    }
}
