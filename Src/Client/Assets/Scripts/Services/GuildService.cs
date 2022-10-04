using Manager;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services{
    public class GuildService : Singleton<GuildService>, System.IDisposable
    {
        public Action<bool> OnGuildCreateResult;
        public Action<List<NGuildInfo>> OnGuildListResult;
        public Action OnGuildUpdate;

        public GuildService()
        {
            MessageDistributer.Instance.Subscribe<GuildCreateResponse>(this.OnGuildCreate);
            MessageDistributer.Instance.Subscribe<GuildListResponse>(this.OnGuildList);
            MessageDistributer.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer.Instance.Subscribe<GuildResponse>(this.OnGuild);
            MessageDistributer.Instance.Subscribe<GuildLeaveResponse>(this.OnGuildLeave);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<GuildCreateResponse>(this.OnGuildCreate);
            MessageDistributer.Instance.Unsubscribe<GuildListResponse>(this.OnGuildList);
            MessageDistributer.Instance.Unsubscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer.Instance.Unsubscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer.Instance.Unsubscribe<GuildResponse>(this.OnGuild);
            MessageDistributer.Instance.Unsubscribe<GuildLeaveResponse>(this.OnGuildLeave);
        }
        internal void Init() { }
        #region Event
        private void OnGuildCreate(object sender, GuildCreateResponse response)
        {
            Debug.LogFormat("OnGuildCreate: result:{0}", response.Result.ToString());
            if (OnGuildCreateResult != null)
            {
                OnGuildCreateResult(response.Result.Equals(Result.Success));
            }
            if (response.Result.Equals(Result.Success))
            {
                GuildManager.Instance.Init(response.guildInfo);
                MessageBox.Show(String.Format("公会【{0}】创建成功", response.guildInfo.GuildName), "公会");
            }
            else
            {
                MessageBox.Show(String.Format("公会【{0}】创建失败", response.guildInfo.GuildName), "公会");
            }
        }

        private void OnGuildList(object sender, GuildListResponse message)
        {
            Debug.LogFormat("OnGuildList: Count:{0}", message.Guilds.Count);
            if(this.OnGuildListResult != null)
            {
                this.OnGuildListResult(message.Guilds);
            }
        }

        private void OnGuildJoinResponse(object sender, GuildJoinResponse message)
        {
            Debug.LogFormat("OnGuildJoinResponse: result:{0}", message.Result.ToString());
            if (message.Result.Equals(Result.Success))
            {
                MessageBox.Show("加入公会成功", "公会");
            }
            else
            {
                MessageBox.Show("加入公会失败", "公会", MessageBoxType.Error);
            }
        }

        private void OnGuildJoinRequest(object sender, GuildJoinRequest message)
        {
            var confirm = MessageBox.Show(String.Format("【{0}】请求加入你的公会", message.Apply.Name), "加入公会请求", MessageBoxType.Confirm, "接受", "拒绝");
            confirm.OnYes = () =>
            {
                GuildService.Instance.SendGuildJoinResponse(true, message);
            };
            confirm.OnNo = () =>
            {
                GuildService.Instance.SendGuildJoinResponse(false, message);
            };
        }

        private void OnGuild(object sender, GuildResponse message)
        {
            Debug.LogFormat("OnGuild: result:{0} ID:{1} Name:{2}", message.Result.ToString(), message.guildInfo.Id, message.guildInfo.GuildName);
            GuildManager.Instance.Init(message.guildInfo);
            if(this.OnGuildUpdate != null)
            {
                this.OnGuildUpdate();
            }
        }

        private void OnGuildLeave(object sender, GuildLeaveResponse message)
        {
            Debug.LogFormat("OnGuildLeave: result:{0}", message.Result.ToString());
            if (message.Result.Equals(Result.Success))
            {
                GuildManager.Instance.Init(null);
                MessageBox.Show("离开公会成功", "公会");
            }
            else
            {
                MessageBox.Show("离开公会失败", "公会", MessageBoxType.Error);
            }
        }
        #endregion
        #region SendMessages
        internal void SendGuildCreate(string guildName, string notice)
        {
            Debug.LogFormat("SendGuildCreate: name:{0} notice:{1}", guildName, notice);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildCreate = new GuildCreateRequest();
            message.Request.guildCreate.GuildName = guildName;
            message.Request.guildCreate.GuildNotice = notice;
            NetClient.Instance.SendMessage(message);
        }

        internal void SendGuildListRequest()
        {
            Debug.Log("SendGuildListRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildList = new GuildListRequest();
            NetClient.Instance.SendMessage(message);
        }

        internal void SendGuildJoinRequest(int id)
        {
            Debug.LogFormat("SendGuildJoinRequest: guildID:{0}", id);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildJoinReq = new GuildJoinRequest();
            message.Request.guildJoinReq.Apply = new NGuildApplyInfo();
            message.Request.guildJoinReq.Apply.GuildId = id;
            NetClient.Instance.SendMessage(message);
        }
        internal void SendGuildJoinResponse(bool accept, GuildJoinRequest request)
        {
            Debug.LogFormat("SendGuildJoinResponse: accept:{0}", accept);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildJoinRes = new GuildJoinResponse();
            message.Request.guildJoinRes.Result = Result.Success;
            message.Request.guildJoinRes.Apply = request.Apply;
            message.Request.guildJoinRes.Apply.Result = accept ? ApplyResult.Accept : ApplyResult.Reject;
            NetClient.Instance.SendMessage(message);
        }
        public void SendGuildLeave()
        {
            Debug.Log("SendGuildLeave");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildLeave = new GuildLeaveRequest();
            NetClient.Instance.SendMessage(message);
        }
        #endregion
    }
}
