using System;
using System.Collections;
using System.Collections.Generic;
using Fenrir.Actors;
using Fenrir.EventBehaviour;
using Fenrir.Managers;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
        [SerializeField] private LayerMask objectsLayerMask;
        [SerializeField] private LayerMask soldierLayerMask;
        [SerializeField] private LayerMask targetLayerMask;
        
        string buildingsName;

        private PlacementSystem _placementSystem;
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
                barrackPos = hit.collider.gameObject.transform.position;
            }

            return barrackPos;
        }

        public GameObject GetObjectsName()
        {
            GameObject selectedObject = null;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = sceneCamera.nearClipPlane;
            Ray ray = sceneCamera.ScreenPointToRay(mousePos);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, objectsLayerMask);
            if (hit.collider != null)
            {
                selectedObject = hit.collider.gameObject;
            }

            return selectedObject;
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

        public GameObject GetSoldier()
        {
            GameObject soldier = null;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = sceneCamera.nearClipPlane;
            Ray ray = sceneCamera.ScreenPointToRay(mousePos);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, soldierLayerMask);
            if (hit.collider != null)
            {
                soldier = hit.collider.gameObject;
            }

            return soldier;
        }

        public GameObject GetTarget()
        {
            GameObject targetObject = null;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = sceneCamera.nearClipPlane;
            Ray ray = sceneCamera.ScreenPointToRay(mousePos);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, targetLayerMask);
            if (hit.collider != null)
            {
                targetObject = hit.collider.gameObject;
            }

            return targetObject;
        }
    }
}