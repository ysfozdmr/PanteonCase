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
    
    private int selectedObjectIndex;
    public GameObject InformationPanel;
    public GameObject SoldiersObject;
    
    public void InformationPanelAction()
    {
        selectedObjectIndex = DataManager.Instance.SelectedObjectIndex;
        buildingImage.sprite = DataManager.Instance._dataBaseSo.objectsData[selectedObjectIndex].Prefab
            .GetComponentInChildren<SpriteRenderer>().sprite;
        buildingText.text=DataManager.Instance._dataBaseSo.objectsData[selectedObjectIndex].Name;
    }

    public void PanelOpen()
    {
        InformationPanelAction();
        if (InputManager.Instance.GetBuildingsName() != null)
        {
            InformationPanel.SetActive(true);
            if (InputManager.Instance.GetBuildingsName().Contains("Barrack"))
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
            InformationPanel.SetActive(false);
            SoldiersObject.SetActive(false);
        }
    }
}
