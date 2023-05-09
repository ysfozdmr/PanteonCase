using System.Collections;
using System.Collections.Generic;
using Fenrir.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RightSidePanel : MonoBehaviour
{
    [SerializeField] private Image buildingImage;
    [SerializeField] private TextMeshProUGUI buildingText;

    [Header("Stat Texts")] [SerializeField]
    private TextMeshProUGUI headerHealthText;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI headerAttackText;
    [SerializeField] private TextMeshProUGUI attackText;

    private int selectedObjectIndex;
    public GameObject InformationPanel;
    public GameObject SoldiersObject;

    private GameObject tempObject = null;
    public void InformationPanelAction()
    {
        if (InputManager.Instance.GetObjectsName() !=null && InputManager.Instance.GetObjectsName() != tempObject )
        {
            tempObject =  InputManager.Instance.GetObjectsName();
        }

        if (tempObject == null)
        {
            return;
        }
        
        if (tempObject.TryGetComponent<SoldierBehaviour>(out var soldierBehaviour))
        {
            selectedObjectIndex =
                DataManager.Instance._dataBaseSo.objectsData.FindIndex(data => data.ID == soldierBehaviour.ID);
            
           
            TextObjectStatus(true);
            healthText.text = soldierBehaviour.Health.ToString();
            attackText.text = soldierBehaviour.AttackPoint.ToString();
        }

        if (tempObject.TryGetComponent<BuildingsBehaviour>(out var buildingsBehaviour))
        {
            selectedObjectIndex =
                DataManager.Instance._dataBaseSo.objectsData.FindIndex(data => data.ID == buildingsBehaviour.ID);
            TextObjectStatus(false);
            healthText.text = buildingsBehaviour.Health.ToString();
        }
        Debug.Log("binaya vutdumm");
        buildingImage.sprite = DataManager.Instance._dataBaseSo.objectsData[selectedObjectIndex].Prefab
            .GetComponentInChildren<SpriteRenderer>().sprite;
        buildingText.text = DataManager.Instance._dataBaseSo.objectsData[selectedObjectIndex].Name;
    }

    void TextObjectStatus(bool isOpen)
    {
        attackText.gameObject.SetActive(isOpen);
        headerAttackText.gameObject.SetActive(isOpen);
    }
    public void PanelOpen()
    {
        InformationPanelAction();
        if (InputManager.Instance.GetObjectsName() != null)
        {
            InformationPanel.SetActive(true);
            if (InputManager.Instance.GetObjectsName().name.Contains("Barrack"))
            {
                SoldiersObject.SetActive(true);
            }
            else
            {
                SoldiersObject.SetActive(false);
            }
        }
        else
        {
            if (!InputManager.Instance.IsPointerOverUI())
            {
                InformationPanel.SetActive(false);
                SoldiersObject.SetActive(false);
            }
        }
    }
}