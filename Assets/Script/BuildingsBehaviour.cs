using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fenrir.Managers;

public class BuildingsBehaviour : MonoBehaviour
{
    public int Health;
    public int AttackPoint;
    public int ID;
    public int index;
    void Start()
    {
        index= DataManager.Instance._dataBaseSo.objectsData.FindIndex(data => data.ID == ID);
        Health=  DataManager.Instance._dataBaseSo.objectsData[index].Health ;
        AttackPoint= DataManager.Instance._dataBaseSo.objectsData[index].AttackPoint ;
    }
    public void TakeDamage(GameObject targetObject)
    {
        if ( targetObject.TryGetComponent<SoldierBehaviour>(out var soldierBehaviour))
        {
            Health -= soldierBehaviour.AttackPoint;
            UIManager.Instance.RightSidePanel.GetComponent<RightSidePanel>().InformationPanelAction();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
