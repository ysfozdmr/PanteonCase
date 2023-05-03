using System;
using System.Collections;
using System.Collections.Generic;
using Fenrir.Actors;
using Fenrir.EventBehaviour;
using Fenrir.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fenrir.Managers
{
    public class InputManager : EventBehaviour<InputManager>
    {
        private Camera sceneCamera;
        private Vector3 lastPos;
        private Vector3 barrackPos;
        [SerializeField] private LayerMask placementLayerMask;
        [SerializeField] private LayerMask barrackLayerMask;


        public event Action OnClicked, OnExit;

        private void Start()
        {
            sceneCamera = DataManager.Instance.SceneCamera;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClicked?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnExit?.Invoke();
            }
        }

        public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

        public Vector3 GetBarrackPosition()
        {
           
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = sceneCamera.nearClipPlane;
            Ray ray = sceneCamera.ScreenPointToRay(mousePos);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, barrackLayerMask);
            if (hit.collider != null)
            {
                Debug.Log(barrackPos);
                barrackPos = hit.collider.gameObject.transform.position;
            }

            return barrackPos;
        }
        public Vector3 GetSelectedMapPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = sceneCamera.nearClipPlane;
            Ray ray = sceneCamera.ScreenPointToRay(mousePos);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, placementLayerMask);
            if (hit.collider != null)
            {
                lastPos = hit.point;
            }

            return lastPos;
        }
    }
}