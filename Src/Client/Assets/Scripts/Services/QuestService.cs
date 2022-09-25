using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network;
using SkillBridge.Message;
using Models;
using Manager;
using UnityEngine;

namespace Services
{
    public class QuestService : Singleton<QuestService>, IDisposable
    {
        public QuestService()
        {
            MessageDistributer.Instance.Subscribe<QuestAcceptResponse>(this.OnQuestAccept);
            MessageDistributer.Instance.Subscribe<QuestSubmitResponse>(this.OnQuestSubmit);
        }
        public void Init() { }
        public void Dispose()
        {
            MessageDistributer.Instance.Subscribe<QuestAcceptResponse>(this.OnQuestAccept);
            MessageDistributer.Instance.Subscribe<QuestSubmitResponse>(this.OnQuestSubmit);
        }
        public bool SendQuestAccept(Quest quest)
        {
            Debug.LogFormat("SendQuestAccept ID:{0}", quest.Define.ID);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.questAccept = new QuestAcceptRequest();
            message.Request.questAccept.QuestId = quest.Define.ID;
            NetClient.Instance.SendMessage(message);
            return true;
        }
        private void OnQuestAccept(object sender, QuestAcceptResponse message)
        {
            Debug.LogFormat("OnQuestAccept result:{0},err:{1}", message.Result.ToString(), message.Errormsg);
            if (message.Result.Equals(Result.Success)){
                QuestManager.Instance.OnQuestAccepted(message.Quest);
            }
            else
            {
                MessageBox.Show("任务接受失败", "Error", MessageBoxType.Error);
            }
        }
        public bool SendQuestSubmit(Quest quest)
        {
            Debug.LogFormat("SendQuestSubmit ID:{0}", quest.Define.ID);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.questSubmit = new QuestSubmitRequest();
            message.Request.questSubmit.QuestId = quest.Define.ID;
            NetClient.Instance.SendMessage(message);
            return true;
        }
        private void OnQuestSubmit(object sender, QuestSubmitResponse message)
        {
            Debug.LogFormat("OnQuestSubmit result:{0},err:{1}", message.Result.ToString(), message.Errormsg);
            if (message.Result.Equals(Result.Success))
            {
                QuestManager.Instance.OnQuestSubmit(message.Quest);
            }
            else
            {
                MessageBox.Show("任务完成失败", "Error", MessageBoxType.Error);
            }
        }


    }
}
