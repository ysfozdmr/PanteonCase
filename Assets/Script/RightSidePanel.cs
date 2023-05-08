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

    public void InformationPanelAction()
    {
        if (InputManager.Instance.GetObjectsName() == null)
        {
            return;
        }

        if (InputManager.Instance.GetObjectsName().TryGetComponent<SoldierBehaviour>(out var soldierBehaviour))
        {
            selectedObjectIndex =
                DataManager.Instance._dataBaseSo.objectsData.FindIndex(data => data.ID == soldierBehaviour.ID);
            
            attackText.gameObject.SetActive(true);
            headerAttackText.gameObject.SetActive(true);
            
            healthText.text = soldierBehaviour.Health.ToString();
            attackText.text = soldierBehaviour.AttackPoint.ToString();
        }

        if (InputManager.Instance.GetObjectsName().TryGetComponent<BuildingsBehaviour>(out var buildingsBehaviour))
        {
            selectedObjectIndex =
                DataManager.Instance._dataBaseSo.objectsData.FindIndex(data => data.ID == buildingsBehaviour.ID);
            attackText.gameObject.SetActive(false);
            headerAttackText.gameObject.SetActive(false);
            healthText.text = buildingsBehaviour.Health.ToString();
        }

        buildingImage.sprite = DataManager.Instance._dataBaseSo.objectsData[selectedObjectIndex].Prefab
            .GetComponentInChildren<SpriteRenderer>().sprite;
        buildingText.text = DataManager.Instance._dataBaseSo.objectsData[selectedObjectIndex].Name;
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