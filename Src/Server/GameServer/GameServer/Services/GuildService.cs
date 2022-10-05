﻿using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    internal class GuildService : Singleton<GuildService>
    {
        public GuildService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildCreateRequest>(this.OnGuildCreate);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildListRequest>(this.OnGuildList);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildRequest>(this.OnGuild);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildLeaveRequest>(this.OnGuildLeave);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildAdminRequest>(this.OnGuildAdmin);
        }
        public void Init()
        {
            GuildManager.Instance.Init();
        }
        private void OnGuildCreate(NetConnection<NetSession> sender, GuildCreateRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildCreate: GuildName:{0} character:{1}:{2}", message.GuildName, character.Id, character.Info.Name);
            sender.Session.Response.guildCreate = new GuildCreateResponse();
            if (character.Guild != null)
            {
                sender.Session.Response.guildCreate.Result = Result.Failed;
                sender.Session.Response.guildCreate.Errormsg = "已经有公会";
                sender.SendResponse();
                return;
            }
            if (GuildManager.Instance.CheckNameExisted(message.GuildName))
            {
                sender.Session.Response.guildCreate.Result = Result.Failed;
                sender.Session.Response.guildCreate.Errormsg = "公会名称已存在";
                sender.SendResponse();
                return;
            }
            GuildManager.Instance.CreateGuild(message.GuildName, message.GuildNotice, character);
            sender.Session.Response.guildCreate.Result = Result.Success;
            sender.Session.Response.guildCreate.guildInfo = character.Guild.GuildInfo(character);
            sender.SendResponse();
        }

        private void OnGuildJoinRequest(NetConnection<NetSession> sender, GuildJoinRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildJoinRequest: GuildId:{0} character:{1}:{2}", message.Apply.GuildId, character.Id, character.Info.Name);
            var guild = GuildManager.Instance.GetGuild(message.Apply.GuildId);
            if(guild == null)
            {
                sender.Session.Response.guildJoinRes=new GuildJoinResponse();
                sender.Session.Response.guildJoinRes.Result= Result.Failed;
                sender.Session.Response.guildJoinRes.Errormsg = "公会不存在";
                sender.SendResponse();
                return;
            }
            message.Apply.characterId = character.Data.ID;
            message.Apply.Name = character.Data.Name;
            message.Apply.Class = character.Data.Class;
            message.Apply.Level = character.Data.Level;
            if (guild.JoinApply(message.Apply))
            {
                var leader = SessionManager.Instance.GetSession(guild.Data.LeaderID);
                if(leader != null)
                {
                    leader.Session.Response.guildJoinReq = message;
                    leader.SendResponse();
                }
                else
                {
                    sender.Session.Response.guildJoinRes = new GuildJoinResponse();
                    sender.Session.Response.guildJoinRes.Result = Result.Failed;
                    sender.Session.Response.guildJoinRes.Errormsg = "请勿重复申请";
                    sender.SendResponse();
                }
            }
        }

        private void OnGuildJoinResponse(NetConnection<NetSession> sender, GuildJoinResponse message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildJoinResponse: GuildId:{0} character:{1}:{2}", message.Apply.GuildId, character.Id, character.Info.Name);
            var guild = GuildManager.Instance.GetGuild(message.Apply.GuildId);
            if(message.Result == Result.Success)
            {
                guild.JoinAppove(message.Apply);
            }
            var requester = SessionManager.Instance.GetSession(message.Apply.characterId);
            if(requester != null)
            {
                requester.Session.Character.Guild = guild;
                requester.Session.Response.guildJoinRes = message;
                requester.Session.Response.guildJoinRes.Result=Result.Success;
                requester.Session.Response.guildJoinRes.Errormsg = "加入公会成功";
                requester.SendResponse();
            }
        }

        private void OnGuildList(NetConnection<NetSession> sender, GuildListRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildList: character:{0}:{1}", character.Id, character.Info.Name);
            sender.Session.Response.guildList = new GuildListResponse();
            sender.Session.Response.guildList.Guilds.AddRange(GuildManager.Instance.GetGuildsInfo());
            sender.Session.Response.guildList.Result = Result.Success;
            sender.SendResponse();
        }

        private void OnGuild(NetConnection<NetSession> sender, GuildRequest message)
        {
            
        }

        private void OnGuildLeave(NetConnection<NetSession> sender, GuildLeaveRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildLeave: character:{0}:{1}", character.Id, character.Info.Name);
            sender.Session.Response.guildLeave = new GuildLeaveResponse();
            character.Guild.Leave(character);
            sender.Session.Response.guildLeave.Result= Result.Success;
            DBService.Instance.Save();
            sender.SendResponse();
        }
        private void OnGuildAdmin(NetConnection<NetSession> sender, GuildAdminRequest message)
        {
            Character character=sender.Session.Character;
            Log.InfoFormat("OnGuildAdmin: character:{0}", character.Id);
            sender.Session.Response.guildAdmin = new GuildAdminResponse();
            if (character.Guild == null)
            {
                sender.Session.Response.guildAdmin.Result = Result.Failed;
                sender.Session.Response.guildAdmin.Errormsg = "数据非法，你将被封号114514小时";
                sender.SendResponse();
                return;
            }
            else
            {
                character.Guild.ExecuteAdmin(message.Command, message.Target, character.Id);
                var target=SessionManager.Instance.GetSession(message.Target);
                if (target != null)
                {
                    target.Session.Response.guildAdmin= new GuildAdminResponse();
                    target.Session.Response.guildAdmin.Result= Result.Success;
                    target.Session.Response.guildAdmin.Request = message;
                    target.SendResponse();
                }
                sender.Session.Response.guildAdmin.Result = Result.Success;
                sender.Session.Response.guildAdmin.Request = message;
                sender.SendResponse();
            }
        }
    }
}
