using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fenrir.EventBehaviour;
using Fenrir.Resources;

namespace Fenrir.Managers
{
    public class DataManager : EventBehaviour<DataManager>
    {
        public ObjectDataBaseSO _dataBaseSo;
        public int SelectedObjectIndex;
        public FXData fXData;
        public LevelDataCapsule levelCapsule;
        public Camera SceneCamera;
        public GameObject RightSidePanel;
    }
}
