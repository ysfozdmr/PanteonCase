using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fenrir.EventBehaviour;
using UnityEngine.UI;

namespace Fenrir.Managers
{
    public class UIManager : EventBehaviour<UIManager>
    {
        public GameObject RightSidePanel;

        public ObjectPooler _objectPooler;

        public GameObject ScroolView;

        private string tag;

        public GameObject spawnObject;

        [SerializeField] private PlacementSystem _placementSystem;

        public GameObject InformationTextObject;


        private void Start()
        {
            SpawnButtons();
            StartCoroutine(InfiniteScrollNum());
        }

        private void GetStartPlacement(int id)
        {
            _placementSystem.StartPlacement(id);
        }
        private void SpawnButtons()
        {
            for (int i = 0; i <= 100; i++)
            {
                if (i % 5 == 0)
                {
                    tag = "Barracks";
                }
                else if (i % 5 == 1)
                {
                    tag = "PowerPlants";
                }
                else if (i % 5 == 2)
                {
                    tag = "Church";
                }
                else if (i % 5 == 3)
                {
                    tag = "House";
                }
                else if (i % 5 == 4)
                {
                    tag = "Workshop";
                }

                int id = i % 5;
                spawnObject = _objectPooler.ObjectSpawnFromPool(tag);
                spawnObject.transform
                    .SetParent(ScroolView.GetComponent<ScrollRect>().content);
                spawnObject.GetComponent<Button>().onClick.AddListener(delegate { GetStartPlacement(id); });
                

            }
        }

        public void InformationTextObjectAction(bool isOpen)
        {
            InformationTextObject.SetActive(isOpen);
        }
        
        IEnumerator InfiniteScrollNum()
        {
            while (true)
            {
                if (ScroolView.GetComponent<ScrollRect>().verticalScrollbar.value < 0.25)
                {
                    SpawnButtons();
                    ScroolView.GetComponent<ScrollRect>().verticalScrollbar.value = 1;
                }
                yield return new WaitForSeconds(0.25f);
            }
            
        }

        public void GetNumerator()
        {
            StartCoroutine(InfiniteScrollNum());
        }
    }
}