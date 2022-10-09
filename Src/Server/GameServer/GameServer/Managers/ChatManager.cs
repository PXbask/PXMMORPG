using Common;
using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GameServer.Managers
{
    internal class ChatManager:Singleton<ChatManager>
    {
        public List<ChatMessage> System=new List<ChatMessage>();
        public List<ChatMessage> World = new List<ChatMessage>();
        public Dictionary<int,List<ChatMessage>> Local = new Dictionary<int,List<ChatMessage>>();
        public Dictionary<int,List<ChatMessage>> Team = new Dictionary<int,List<ChatMessage>>();
        public Dictionary<int,List<ChatMessage>> Guild = new Dictionary<int,List<ChatMessage>>();

        public void Init() { }
        public void AddMessage(Character from,ChatMessage message)
        {
            message.FromId = from.Id;
            message.FromName = from.Name;
            message.Time = TimeUtil.timestamp;
            switch (message.Channel)
            {
                case ChatChannel.Local:
                    this.AddLocalMessage(from.Info.mapId, message);
                    break;
                case ChatChannel.World:
                    this.AddWorldMessage(message);
                    break;
                case ChatChannel.System:
                    this.AddSystemMessage(message);
                    break;
                case ChatChannel.Team:
                    this.AddTeamlMessage(from.team.Id, message);
                    break;
                case ChatChannel.Guild:
                    this.AddGuildMessage(from.Guild.Id, message);
                    break;
            }
        }

        private void AddLocalMessage(int mapId, ChatMessage message)
        {
            if(!Local.TryGetValue(mapId, out List<ChatMessage> messages))
            {
                messages = new List<ChatMessage>();
                Local.Add(mapId, messages);
            }
            messages.Add(message);
        }

        private void AddWorldMessage(ChatMessage message)
        {
            this.World.Add(message);
        }

        private void AddSystemMessage(ChatMessage message)
        {
            this.System.Add(message);
        }

        private void AddTeamlMessage(int id, ChatMessage message)
        {
            if (!Team.TryGetValue(id, out List<ChatMessage> messages))
            {
                messages = new List<ChatMessage>();
                Team.Add(id, messages);
            }
            messages.Add(message);
        }

        private void AddGuildMessage(int id, ChatMessage message)
        {
            if (!Guild.TryGetValue(id, out List<ChatMessage> messages))
            {
                messages = new List<ChatMessage>();
                Guild.Add(id, messages);
            }
            messages.Add(message);
        }
        public int GetLocalMessages(int mapId,int idx,List<ChatMessage> result)
        {
            if(!this.Local.TryGetValue(mapId, out List<ChatMessage> messages))
            {
                return 0;
            }
            return GetNewMessages(idx, result, messages);
        }
        public int GetWorldMessages(int idx, List<ChatMessage> result)
        {
            return GetNewMessages(idx, result, this.World);
        }
        public int GetSystemMessages(int idx, List<ChatMessage> result)
        {
            return GetNewMessages(idx, result, this.System);
        }
        public int GetTeamMessages(int mapId, int idx, List<ChatMessage> result)
        {
            if (!this.Team.TryGetValue(mapId, out List<ChatMessage> messages))
            {
                return 0;
            }
            return GetNewMessages(idx, result, messages);
        }
        public int GetGuildMessages(int mapId, int idx, List<ChatMessage> result)
        {
            if (!this.Guild.TryGetValue(mapId, out List<ChatMessage> messages))
            {
                return 0;
            }
            return GetNewMessages(idx, result, messages);
        }

        private int GetNewMessages(int idx, List<ChatMessage> result, List<ChatMessage> messages)
        {
            if (idx == 0)
            {
                if (messages.Count > GameDefine.MaxChatRecordNums)
                {
                    idx = messages.Count - GameDefine.MaxChatRecordNums;
                }
            }
            for (; idx < messages.Count; idx++)
            {
                result.Add(messages[idx]);
            }
            return idx;
        }
    }


}
