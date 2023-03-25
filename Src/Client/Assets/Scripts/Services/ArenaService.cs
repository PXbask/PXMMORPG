using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Services
{
    internal class ArenaService : Singleton<ArenaService>,IDisposable
    {
        public ArenaService()
        {
            MessageDistributer.Instance.Subscribe<ArenaChallengeRequest>(this.OnArenaChallengeReq);
            MessageDistributer.Instance.Subscribe<ArenaChallengeResponse>(this.OnArenaChallengeRes);
            MessageDistributer.Instance.Subscribe<ArenaBeginResponse>(this.OnArenaBegin);
            MessageDistributer.Instance.Subscribe<ArenaEndResponse>(this.OnArenaEnd);
        }
        public void Init()
        {

        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ArenaChallengeRequest>(this.OnArenaChallengeReq);
            MessageDistributer.Instance.Unsubscribe<ArenaChallengeResponse>(this.OnArenaChallengeRes);
            MessageDistributer.Instance.Unsubscribe<ArenaBeginResponse>(this.OnArenaBegin);
            MessageDistributer.Instance.Unsubscribe<ArenaEndResponse>(this.OnArenaEnd);
        }
        #region Event
        private void OnArenaChallengeReq(object sender, ArenaChallengeRequest message)
        {
            Debug.Log("OnArenaChallengeReq");
            var confirm = MessageBox.Show(string.Format("{0}邀请你竞技场对战", message.ArenaInfo.Red.Name), "竞技场对战", MessageBoxType.Confirm, "同意", "拒绝");
            confirm.OnYes = () =>
            {
                ArenaService.Instance.SendArenaChallengeResponse(true, message);
            };
            confirm.OnNo = () =>
            {
                ArenaService.Instance.SendArenaChallengeResponse(false, message);
            };
        }

        private void OnArenaChallengeRes(object sender, ArenaChallengeResponse message)
        {
            Debug.Log("OnArenaChallengeRes");
            if (message.Result.Equals(Result.Failed))
                MessageBox.Show(message.Errormsg, "拒绝挑战");
        }

        private void OnArenaBegin(object sender, ArenaBeginResponse message)
        {
            Debug.Log("OnArenaBegin");
            ArenaManager.Instance.EnterArena(message.ArenaInfo);
        }

        private void OnArenaEnd(object sender, ArenaEndResponse message)
        {
            Debug.Log("OnArenaEnd");
            ArenaManager.Instance.ExitArena(message.ArenaInfo);
        }
        #endregion
        #region SendMessages
        public void SendArenaChallengeRequest(int friendId, string friendName)
        {
            Debug.LogFormat("SendArenaChallengeRequest:friendId:{0},friendName:{1}", friendId, friendName);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.ArenaChallengeReq = new ArenaChallengeRequest();
            message.Request.ArenaChallengeReq.ArenaInfo = new ArenaInfo();
            message.Request.ArenaChallengeReq.ArenaInfo.Red = new ArenaPlayer()
            {
                EntityId= Models.User.Instance.CurrentCharacterInfo.Id,
                Name = Models.User.Instance.CurrentCharacterInfo.Name,
            };
            message.Request.ArenaChallengeReq.ArenaInfo.Blue = new ArenaPlayer()
            {
                EntityId = friendId,
                Name = friendName,
            };
            NetClient.Instance.SendMessage(message);
        }
        public void SendArenaChallengeResponse(bool accept,ArenaChallengeRequest request)
        {
            Debug.LogFormat("SendArenaChallengeResponse:accept:{0}", accept);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.ArenaChallengeRes = new ArenaChallengeResponse();
            message.Request.ArenaChallengeRes.Result = accept ? Result.Success : Result.Failed;
            message.Request.ArenaChallengeRes.Errormsg = accept ? "" : "对方拒绝了你的挑战";
            message.Request.ArenaChallengeRes.ArenaInfo = request.ArenaInfo;
            NetClient.Instance.SendMessage(message);
        }
        #endregion

    }
}
