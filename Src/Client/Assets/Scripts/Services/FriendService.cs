using Manager;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services {
    public class FriendService : Singleton<FriendService>, IDisposable
    {
        public Action OnFriendUpdate;
        public void Init() { }
        public FriendService()
        {
            MessageDistributer.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer.Instance.Subscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Subscribe<FriendRemoveResponse>(this.OnFriendRemove);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer.Instance.Unsubscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer.Instance.Unsubscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Unsubscribe<FriendRemoveResponse>(this.OnFriendRemove);
        }
        #region Event
        private void OnFriendAddRequest(object sender, FriendAddRequest message)
        {
            var confirm = MessageBox.Show(String.Format("【{0}】请求加你为好友", message.FromName), "好友请求", MessageBoxType.Confirm, "接受", "拒绝");
            confirm.OnYes = () =>
            {
                FriendService.Instance.SendFriendAddResponse(true, message);
            };
            confirm.OnNo = () =>
            {
                FriendService.Instance.SendFriendAddResponse(false, message);
            };
        }
        private void OnFriendAddResponse(object sender, FriendAddResponse message)
        {
            if (message.Result == Result.Success)
                MessageBox.Show(message.Request.ToName + "接受了你的请求", btnOK: "添加好友成功");
            else
                MessageBox.Show(message.Errormsg, btnOK: "添加好友失败");

        }
        private void OnFriendList(object sender, FriendListResponse message)
        {
            Debug.Log("OnFriendList");
            FriendManager.Instance.Init(message.Friends);
            if (this.OnFriendUpdate != null)
                this.OnFriendUpdate();
        }
        private void OnFriendRemove(object sender, FriendRemoveResponse message)
        {
            if (message.Result == Result.Success)
                MessageBox.Show("删除成功", "删除好友");
            else
                MessageBox.Show("删除失败", "删除好友", MessageBoxType.Error);
        }
        #endregion

        #region SendMessages
        public void SendFriendAddRequest(int friendId, string friendName)
        {
            Debug.LogFormat("SendFriendAddRequest:friendId:{0},friendName:{1}", friendId, friendName);
            NetMessage message=new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendAddReq=new FriendAddRequest();
            message.Request.friendAddReq.FromId = User.Instance.CurrentCharacterInfo.Id;
            message.Request.friendAddReq.FromName = User.Instance.CurrentCharacterInfo.Name;
            message.Request.friendAddReq.ToId = friendId;
            message.Request.friendAddReq.ToName = friendName;
            NetClient.Instance.SendMessage(message);
        }
        public void SendFriendAddResponse(bool accept, FriendAddRequest request)
        {
            Debug.LogFormat("SendFriendAddResponse: accept:{0}", accept.ToString());
            NetMessage message=new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendAddRes=new FriendAddResponse();
            message.Request.friendAddRes.Result = accept ? Result.Success : Result.Failed;
            message.Request.friendAddRes.Errormsg = accept ? "同意" : "拒绝";
            message.Request.friendAddRes.Request = request;
            NetClient.Instance.SendMessage(message);
        }
        internal void SendFriendRemoveRequest(int id, int friendId)
        {
            Debug.Log("SendFriendRemoveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendRemove = new FriendRemoveRequest();
            message.Request.friendRemove.Id = id;
            message.Request.friendRemove.friendId = friendId;
            NetClient.Instance.SendMessage(message);
        }
        #endregion
    }
}


