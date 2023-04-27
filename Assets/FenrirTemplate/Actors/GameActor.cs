using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fenrir.EventBehaviour;

namespace Fenrir.Actors
{
    public abstract class GameActor<T2> : Actor where T2 : EventBehaviour<T2>
    {
        T2 driver;

        public System.Action updateWork;
        public System.Action fixedUpdateWork;

        public override sealed void Awake()
        {
            driver = FindObjectOfType<T2>();
            driver.AddMono(this);
            ActorAwake();
        }
        public override sealed void Start()
        {
            ActorStart();
        }
        public override sealed void Update()
        {
            updateWork?.Invoke();
            ActorUpdate();
        }
        public override sealed void FixedUpdate()
        {
            fixedUpdateWork?.Invoke();
            ActorFixedUpdate();
        }
        public override sealed void OnDestroy()
        {
            driver?.RemoveMono(this);
            ActorOnDestroy();
        }
        
        public virtual void ActorStart() { }
        public virtual void ActorAwake() { }
        public virtual void ActorOnDestroy() { }
        public virtual void ActorUpdate() { }
        public virtual void ActorFixedUpdate() { }
    }
}