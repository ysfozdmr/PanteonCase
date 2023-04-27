using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fenrir.Actors
{
    public abstract class Actor : MonoBehaviour
    {
        public abstract void Start();
        public abstract void Awake();
        public abstract void OnDestroy();
        public abstract void Update();
        public abstract void FixedUpdate();
    }
}