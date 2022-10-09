using Manager;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    internal class ChatService : Singleton<ChatService>,IDisposable
    {
        public ChatService()
        {
            MessageDistributer.Instance.Subscribe<ChatResponse>(this.OnChat);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ChatResponse>(this.OnChat);
        }
        public void Init()
        {
            ChatManager.Instance.Init();
        }
        internal void SendChat(ChatChannel sendChannel, string msg, int toId, string toName)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.Chat = new ChatRequest();
            message.Request.Chat.Message = new ChatMessage
            {
                Channel = sendChannel,
                Message = msg,
                ToId = toId,
                ToName = toName
            };
            NetClient.Instance.SendMessage(message);
        }
        private void OnChat(object sender, ChatResponse message)
        {
            if (message.Result.Equals(Result.Success))
            {
                ChatManager.Instance.AddMessages(ChatChannel.Local, message.localMessages);
                ChatManager.Instance.AddMessages(ChatChannel.World, message.worldMessages);
                ChatManager.Instance.AddMessages(ChatChannel.System, message.systemMssages);
                ChatManager.Instance.AddMessages(ChatChannel.Team, message.teamMessages);
                ChatManager.Instance.AddMessages(ChatChannel.Guild, message.guildMessages);
                ChatManager.Instance.AddMessages(ChatChannel.Private, message.privateMessages);
            }
            else
            {
                ChatManager.Instance.AddSystemMessage(message.Errormsg);
            }
        }
    }
}
