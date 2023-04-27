using UnityEngine;
using System.Collections.Generic;
using System;
using Fenrir.Managers;
using Fenrir.Resources;

namespace Fenrir.Utilities
{
    public static class Utility
    {
        public static GameObject Display(this ParticleFXDisplayer particleFXDisplayer)
        {
            FXData.ParticleData particleData = DataManager.Instance.fXData.particleDatas.Find(x => x.ID.Equals(particleFXDisplayer.particleID, StringComparison.OrdinalIgnoreCase));
            GameObject created = GameObject.Instantiate(particleData.Prefab, particleFXDisplayer.position, particleFXDisplayer.rotation);
            GameObject.Destroy(created, particleFXDisplayer.destroyTime + 0.1f);
            return created;
        }
        public static void Display(this AudioFXDisplayer audioFXDisplayer)
        {

        }
    }
}
