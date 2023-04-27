
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using Fenrir.EventBehaviour.Attributes;

namespace Fenrir.EventBehaviour
{
    public abstract class EventBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (!instance)
                {
                    instance = FindObjectOfType<T>();
                }
                return instance;
            }
        }
        private List<Data> events = new List<Data>();
        public void PushEvent(int eventID, params object[] data) => events.FindAll(x => x.Id == eventID).ForEach(x => x.Exacute(data));

        public void AddMono(MonoBehaviour mono)
        {
            var current = mono.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Where(m => m.GetCustomAttributes(typeof(GE), true).Length > 0).ToList();
            foreach (var item in current)
            {
                GE gE = item.GetCustomAttribute<GE>();
                Data newx = new Data(gE.ID, mono, item);
                events.Add(newx);
            }
        }
        public void RemoveMono(MonoBehaviour mono)
        {
            events.RemoveAll(x => x == mono);
        }

        public class Data
        {
            public Data(int eventID, MonoBehaviour _behaviour, MethodInfo _methodInfo)
            {
                Id = eventID;
                monoBehaviour = _behaviour;
                methodInfo = _methodInfo;
            }
            private MonoBehaviour monoBehaviour;
            private MethodInfo methodInfo;
            public int Id;
            public void Exacute(params object[] data) => methodInfo.Invoke(monoBehaviour, data);
            public static implicit operator MonoBehaviour(Data data) => data.monoBehaviour;
        }
    }


}



