using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameServer;
using GameServer.Entities;
using SkillBridge.Message;

namespace Network
{
    internal class NetSession : INetSession
    {
        public TUser User { get; set; }
        public Character Character { get; set; }
        public NEntity Entity { get; set; }
        public IPostResponser PostResponser { get; set; }

        internal void Disconnected()
        {
            this.PostResponser = null;
            if (Character != null)
                GameServer.Services.UserService.Instance.CharacterLeave(Character);
        }
        NetMessage response;
        public NetMessageResponse Response
        {
            get
            {
                if(response == null)
                {
                    response = new NetMessage();
                }
                if(response.Response == null)
                {
                    response.Response=new NetMessageResponse();
                }
                return response.Response;
            }
        }
        public byte[] GetResponse()
        {
            if (response != null)
            {
                if(PostResponser != null)
                {
                    PostResponser.PostProcess(Response);
                }
                byte[] data = PackageHandler.PackMessage(response);
                response = null;
                return data;
            }
            return null;
        }
    }
}
