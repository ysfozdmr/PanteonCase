using System;
using System.Collections;
using System.Collections.Generic;
using Fenrir.Managers;
using UnityEngine;

public class SoldierBehaviour : MonoBehaviour
{
    public List<Vector3Int> _pathFinding;

    public int Health;
    public int AttackPoint;
    public int ID;
    public int index;
    
    private void Awake()
    {
        index= DataManager.Instance._dataBaseSo.objectsData.FindIndex(data => data.ID == ID);
        Health=  DataManager.Instance._dataBaseSo.objectsData[index].Health ;
        AttackPoint= DataManager.Instance._dataBaseSo.objectsData[index].AttackPoint ;
    }

    public void Attack(GameObject targetObject)
    {
        if ( targetObject.TryGetComponent<SoldierBehaviour>(out var soldierBehaviour))
        {
            soldierBehaviour.TakeDamage(gameObject);
        }

        if (targetObject.TryGetComponent<BuildingsBehaviour>(out var buildingsBehaviour))
        {
            buildingsBehaviour.TakeDamage(gameObject);
        }
    }

    public void TakeDamage(GameObject targetObject)
    {
        if ( targetObject.TryGetComponent<SoldierBehaviour>(out var soldierBehaviour))
        {
            Health -= soldierBehaviour.AttackPoint;
            UIManager.Instance.RightSidePanel.GetComponent<RightSidePanel>().InformationPanelAction();
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    

}
