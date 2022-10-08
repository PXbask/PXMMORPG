using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace Manager
{
    public class ChatManager : Singleton<ChatManager>
    {
        public enum LocalChannel
        {
            All=0,
            Local=1,
            World=2,
            Team=3,
            Guild=4,
            Private=5,
        }
        private ChatChannel[] ChannelFilter = new ChatChannel[6]
        {
            ChatChannel.Local|ChatChannel.World|ChatChannel.Team|ChatChannel.Guild|ChatChannel.Private,
            ChatChannel.Local,
            ChatChannel.World,
            ChatChannel.Team,
            ChatChannel.Guild,
            ChatChannel.Private
        };
        internal void StartPrivateChat(int targetId, string targetName)
        {
            this.PrivateID = targetId;
            this.PrivateName = targetName;
            this.sendChannel = LocalChannel.Private;
            if (this.OnChat != null)
                this.OnChat();
        }
        internal LocalChannel displayChannel;
        internal Action OnChat;
        internal LocalChannel sendChannel;
        internal string PrivateName;
        internal int PrivateID;
        internal ChatChannel SendChannel;
        private List<ChatMessage> Messages = new List<ChatMessage>();

        internal void SendChat(string message, int toId = 0, string toName = "")
        {
            this.Messages.Add(new ChatMessage
            {
                Channel = ChatChannel.Local,
                Message = message,
                ToId = toId,
                ToName = toName,
                FromId = User.Instance.CurrentCharacter.Id,
                FromName = User.Instance.CurrentCharacter.Name,
            });
        }

        internal bool SetSendChannel(LocalChannel channel)
        {
            if(channel == LocalChannel.Team)
            {
                if (User.Instance.TeamInfo == null)
                {
                    this.AddSystemMessage("你没有加入任何队伍");
                    return false;
                }
            }
            if (channel == LocalChannel.Guild)
            {
                if (User.Instance.CurrentCharacter.Guild == null)
                {
                    this.AddSystemMessage("你没有加入任何公会");
                    return false;
                }
            }
            this.sendChannel= channel;
            Debug.LogFormat("Set Channel:{0}", this.sendChannel);
            return true;
        }

        private void AddSystemMessage(string message,string from="")
        {
            this.Messages.Add(new ChatMessage
            {
                Channel = ChatChannel.System,
                Message = message,
                FromName = from,
            });
            if(this.OnChat!=null)
                this.OnChat();
        }
        internal string GetCurrentMessages()
        {
            StringBuilder sb=new StringBuilder();
            foreach(ChatMessage message in this.Messages)
            {
                sb.AppendLine(FormatMessage(message));
            }
            return sb.ToString();
        }

        private string FormatMessage(ChatMessage message)
        {
            switch (message.Channel)
            {
                case ChatChannel.Local:
                    return String.Format("[本地]{0} {1}", FormatFromPlayer(message), message.Message);
                case ChatChannel.World:
                    return String.Format("<color=cyan>[世界]{0} {1}</color>", FormatFromPlayer(message), message.Message);
                case ChatChannel.System:
                    return String.Format("<color=yellow>[系统]{0}</color>", message.Message);
                case ChatChannel.Private:
                    return String.Format("<color=magenta>[私聊]{0} {1}</color>", FormatFromPlayer(message), message.Message);
                case ChatChannel.Team:
                    return String.Format("<color=green>[队伍]{0} {1}</color>", FormatFromPlayer(message), message.Message);
                case ChatChannel.Guild:
                    return String.Format("<color=blue>[公会]{0} {1}</color>", FormatFromPlayer(message), message.Message);
            }
            return String.Empty;
        }

        private object FormatFromPlayer(ChatMessage message)
        {
            if (message.FromId == User.Instance.CurrentCharacter.Id)
            {
                return "<a name=\"\" class=\"player\">[你]</a>";
            }
            else
            {
                return string.Format("<a name=\"c:{0}:{1}\" class=\"player\">[{2}]</a>", message.FromId, message.FromName, message.FromName);
            }
        }
    }
}



