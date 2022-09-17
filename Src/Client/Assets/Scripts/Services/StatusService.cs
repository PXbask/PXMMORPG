using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using Models;

namespace Services
{
    class StatusService : Singleton<StatusService>, IDisposable
    {
        public delegate bool StatusNotifyHandler(NStatus status);
        Dictionary<StatusType, StatusNotifyHandler> eventMap=new Dictionary<StatusType, StatusNotifyHandler>();
        public void Init() { }
        public StatusService()
        {
            MessageDistributer.Instance.Subscribe<StatusNotify>(this.OnStatusNotify);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<StatusNotify>(this.OnStatusNotify);
        }

        public void RegisterStatusNotify(StatusType function, StatusNotifyHandler action)
        {
            if (!eventMap.ContainsKey(function))
                eventMap.Add(function, action);
            else
                eventMap[function] += action;
        }
        private void OnStatusNotify(object sender, StatusNotify message)
        {
            foreach(NStatus status in message.Status)
            {
                Notify(status);
            }
        }

        private void Notify(NStatus status)
        {
            Debug.LogFormat("StatusNotify:[{0}][{1}]{2}:{3}", status.Type, status.Action, status.Id, status.Value);
            if(status.Type == StatusType.Money)
            {
                if (status.Action == StatusAction.Add)
                    User.Instance.AddGold(status.Value);
                else if (status.Action == StatusAction.Delete)
                    User.Instance.AddGold(-status.Value);
            }
            StatusNotifyHandler handler;
            if(eventMap.TryGetValue(status.Type, out handler))
            {
                handler(status);
            }
        }
    }
}

