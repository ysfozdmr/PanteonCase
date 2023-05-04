using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public static ObjectPooler instance;

    public GameObject PoolObjects;
    private void Awake()
    {
        instance = this;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict;
    void Start()
    {
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> ObjectPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                ObjectPool.Enqueue(obj);
                obj.transform.SetParent(PoolObjects.transform);
            }
            poolDict.Add(pool.tag, ObjectPool);
        }
    }

    public void Spawn()
    {
        
    }
    public GameObject ObjectSpawnFromPool(string tag)
    {

        if (!poolDict.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag" + tag + "doesn't excist.");
            return null;
        }
        GameObject objectToSpawn = poolDict[tag].Dequeue();

        objectToSpawn.SetActive(true);
        poolDict[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }


}
