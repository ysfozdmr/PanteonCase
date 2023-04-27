using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fenrir.Actors;


namespace Fenrir.Resources
{
    [CreateAssetMenu(fileName = "Level Data", menuName = "Fenrir/Level Data")]
    public class LevelData : ScriptableObject
    {
        public LevelActor LevelPrefab;
    }
    
}