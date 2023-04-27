using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fenrir.Resources
{

    [CreateAssetMenu(fileName = "Level Capsule", menuName = "Fenrir/Level Data Capsule")]
    public class LevelDataCapsule : ScriptableObject
    {
        public List<LevelData> Levels;

        public GameObject LevelPrefab(int currentLevel)
        {
            GameObject result = null;
            int final = currentLevel % Levels.Count;
            result = Levels[final].LevelPrefab.gameObject;
            return result;
        }
    }
}
