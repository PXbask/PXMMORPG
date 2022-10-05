﻿using GameServer.Entities;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using GameServer.Managers;
using Utils;

namespace GameServer.Models
{
    internal class Guild
    {
        public int Id { get { return this.Data.Id; } }
        public string Name { get { return this.Data.Name; } }
        public double timestamp;
        public TGuild Data;
        public Guild(TGuild guild)
        {
            this.Data = guild;
        }
        /// <summary>
        /// 加入公会申请
        /// </summary>
        /// <param name="apply"></param>
        /// <returns></returns>
        internal bool JoinApply(NGuildApplyInfo apply)
        {
            var oldApply = this.Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId);
            if(oldApply != null) return false;

            var dbApply = DBService.Instance.Entities.GuildApplies.Create();
            dbApply.TGuildId = apply.GuildId;
            dbApply.CharacterId = apply.characterId;
            dbApply.Name = apply.Name;
            dbApply.Class = apply.Class;
            dbApply.Level = apply.Level;
            dbApply.ApplyTime = DateTime.Now;

            DBService.Instance.Entities.GuildApplies.Add(dbApply);
            this.Data.Applies.Add(dbApply);
            return true;
        }
        internal bool JoinAppove(NGuildApplyInfo apply)
        {
            var oldApply=this.Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId && v.Result==0);
            if (oldApply == null) return false;

            oldApply.Result = (int)apply.Result;
            if (apply.Result.Equals(ApplyResult.Accept))
            {
                this.AddMember(apply.characterId, apply.Name, apply.Class, apply.Level, GuildTitle.None);
            }
            DBService.Instance.Save();
            this.timestamp = TimeUtil.timestamp;
            return true;
        }

        public void AddMember(int characterId, string name, int @class, int level, GuildTitle title)
        {
            DateTime now = DateTime.Now;
            TGuildMember member = new TGuildMember
            {
                CharacterId = characterId,
                Name = name,
                Class = @class,
                Level = level,
                Title = (int)title,
                JoinTime = now,
                LastTime = now,
            };
            this.Data.Members.Add(member);
            var character=CharacterManager.Instance.GetCharacter(characterId);
            if (character != null)
            {
                character.Data.GuildID = this.Id;
            }
            else
            {
                TCharacter dbchar = DBService.Instance.Entities.Characters.SingleOrDefault(v=>v.ID == characterId);
                dbchar.GuildID = this.Id;
            }
            timestamp = TimeUtil.timestamp;
        }

        internal NGuildInfo GuildInfo(Character character)
        {
            NGuildInfo info = new NGuildInfo
            {
                Id = this.Id,
                GuildName = this.Name,
                Notice = this.Data.Notice,
                leaderId = this.Data.LeaderID,
                leaderName = this.Data.LeaderName,
                createTime = (long)TimeUtil.GetTimestamp(this.Data.CreateTime),
                memberCount = this.Data.Members.Count,
            };
            if(character != null)
            {
                info.Members.AddRange(GetMemberInfos());
                if (character.Id == this.Data.LeaderID)
                    info.Applies.AddRange(GetApplyInfos());
            }
            return info;
        }

        private List<NGuildMemberInfo> GetMemberInfos()
        {
            List<NGuildMemberInfo> members=new List<NGuildMemberInfo>();
            foreach (var member in this.Data.Members)
            {
                var memberInfo = new NGuildMemberInfo
                {
                    Id = member.Id,
                    characterId = member.CharacterId,
                    Title = (GuildTitle)member.Title,
                    joinTime = (long)TimeUtil.GetTimestamp(member.JoinTime),
                    lastTime = (long)TimeUtil.GetTimestamp(member.LastTime),
                };
                var character = CharacterManager.Instance.GetCharacter(member.CharacterId);
                if (character != null)
                {
                    memberInfo.Info=character.GetBasicInfo();
                    memberInfo.Status = 1;

                    member.Level=character.Data.Level;
                    member.Name = character.Data.Name;
                    member.LastTime = DateTime.Now;
                }
                else
                {
                    memberInfo.Info = this.GetMemberInfo(member);
                    memberInfo.Status = 0;
                }
                members.Add(memberInfo);
            }
            timestamp = TimeUtil.timestamp;
            return members;
        }

        private NCharacterInfo GetMemberInfo(TGuildMember member)
        {
            return new NCharacterInfo
            {
                Id = member.Id,
                Name = member.Name,
                Class = (CharacterClass)member.Class,
                Level = member.Level,
            };
        }

        private List<NGuildApplyInfo> GetApplyInfos()
        {
            List<NGuildApplyInfo> applies=new List<NGuildApplyInfo>();
            foreach (var apply in this.Data.Applies)
            {
                if (apply.Result != (int)ApplyResult.None) continue;
                applies.Add(new NGuildApplyInfo
                {
                    characterId=apply.CharacterId,
                    GuildId=apply.TGuildId,
                    Class=apply.Class,
                    Level=apply.Level,
                    Name=apply.Name,
                    Result=(ApplyResult)apply.Result,
                });
            }
            return applies;
        }

        public void PostProcess(Character from ,NetMessageResponse response)
        {
            if (response.Guild == null)
            {
                response.Guild = new GuildResponse();
                response.Guild.Result = Result.Success;
                response.Guild.guildInfo = GuildInfo(from);
            }
        }
        private TGuildMember GetDBMember(int characterId)
        {
            foreach(var member in this.Data.Members)
            {
                if(member.CharacterId == characterId)
                {
                    return member;
                }
            }
            return null;
        }
        internal void ExecuteAdmin(GuildAdminCommand command, int targetId, int sourceId)
        {
            var target = GetDBMember(targetId);
            var source = GetDBMember(sourceId);
            switch (command)
            {
                case GuildAdminCommand.Promote:
                    target.Title = (int)GuildTitle.VicePresident;
                    break;
                case GuildAdminCommand.Depost:
                    target.Title= (int)GuildTitle.None;
                    break;
                case GuildAdminCommand.Transfer:
                    target.Title = (int)GuildTitle.President;
                    source.Title = (int)GuildTitle.None;
                    this.Data.LeaderID = targetId;
                    this.Data.LeaderName = target.Name;
                    break;
                case GuildAdminCommand.Kickout:
                    //TODO:自己填充
                    break;
            }
            DBService.Instance.Save();
            timestamp = TimeUtil.timestamp;
        }

        internal void Leave(Character character)
        {
            //TODO:自己填充
        }
    }
}
