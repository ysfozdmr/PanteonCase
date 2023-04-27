
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fenrir.Resources
{
    [CreateAssetMenu(fileName = "FX Data", menuName = "Fenrir/FX Data")]
    public class FXData : ScriptableObject
    {
        public List<ParticleData> particleDatas;
        public List<SoundData> soundDatas;

        [System.Serializable]
        public struct ParticleData
        {
            public string ID;
            public GameObject Prefab;
        }
        [System.Serializable]
        public struct SoundData
        {
            public string ID;
            public AudioClip Sound;
        }

    }
    public struct ParticleFXDisplayer
    {
        public string particleID;
        public float destroyTime;
        public Vector3 position;
        public Quaternion rotation;
    }
    public struct AudioFXDisplayer
    {
        public string audioID;
        public Vector3 position;
    }
}
