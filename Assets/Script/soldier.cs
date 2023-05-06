using System;
using System.Collections;
using System.Collections.Generic;
using Fenrir.Managers;
using UnityEngine;

public class soldier : MonoBehaviour
{
    public List<Vector3Int> _pathFinding;

    private void Awake()
    {
       
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Movement()
    {
        StartCoroutine(Walk());
    }
    
    IEnumerator Walk()
    {
        int tempCount = _pathFinding.Count;
        Debug.Log( _pathFinding.Count);
        for (int i = 0; i < tempCount; i++)
        {
            float x = 1;

                while (x >=0)
                {
                    transform.position = Vector3.Lerp(transform.position,_pathFinding[0], 5f*Time.deltaTime );
                    x -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }        
                _pathFinding.RemoveAt(0);
        }
       
    }

}
