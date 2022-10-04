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

namespace GameServer.Models
{
    internal class Guild
    {
        public int Id { get { return this.Data.Id; } }
        private Character Leader;
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
            this.timestamp = Time.timestamp;
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
            timestamp = Time.timestamp;
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
                createTime = (long)Time.GetTimestamp(this.Data.CreateTime),
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
                    joinTime = (long)Time.GetTimestamp(member.JoinTime),
                    lastTime = (long)Time.GetTimestamp(member.LastTime),
                };
                var character = CharacterManager.Instance.GetCharacter(member.CharacterId);
                if (character != null)
                {
                    memberInfo.Info=character.GetBasicInfo();
                    memberInfo.Status = 1;

                    member.Level=character.Data.Level;
                    member.Name = character.Data.Name;
                    member.LastTime = DateTime.Now;
                    if (member.Id == this.Data.LeaderID)
                        this.Leader = character;
                }
                else
                {
                    memberInfo.Info = this.GetMemberInfo(member);
                    memberInfo.Status = 0;
                    if (member.Id == this.Data.LeaderID)
                        this.Leader = null;
                }
                members.Add(memberInfo);
            }
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

        internal void Leave(Character character)
        {
            //TODO:自己填充
        }
    }
}
