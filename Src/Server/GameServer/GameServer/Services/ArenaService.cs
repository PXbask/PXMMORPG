using Common;
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
    class ArenaService : Singleton<ArenaService>
    {
        public ArenaService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ArenaChallengeRequest>(this.OnArenaChallengeRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ArenaChallengeResponse>(this.OnArenaChallengeResponse);
        }
        public void Init() {
            ArenaManager.Instance.Init();
        }
        private void OnArenaChallengeRequest(NetConnection<NetSession> sender, ArenaChallengeRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnArenaChallengeRequest::RedId:{0} RedName:{1} BlueID:{2} BlueName:{3}",
                message.ArenaInfo.Red.EntityId, message.ArenaInfo.Red.Name, message.ArenaInfo.Blue.EntityId, message.ArenaInfo.Blue.Name);
            NetConnection<NetSession> blue = null;
            if (message.ArenaInfo.Blue.EntityId > 0)
            {
                blue = SessionManager.Instance.GetSession(message.ArenaInfo.Blue.EntityId);
            }
            if (blue == null)
            {
                sender.Session.Response.arenaChallengeRes = new ArenaChallengeResponse();
                sender.Session.Response.arenaChallengeRes.Result = Result.Failed;
                sender.Session.Response.arenaChallengeRes.Errormsg = "好友不存在或不在线";
                sender.SendResponse();
                return;
            }
            Log.InfoFormat("OnArenaChallengeRequest::RedId:{0} RedName:{1} BlueID:{2} BlueName:{3}",
                message.ArenaInfo.Red.EntityId, message.ArenaInfo.Red.Name, message.ArenaInfo.Blue.EntityId, message.ArenaInfo.Blue.Name);
            blue.Session.Response.arenaChallengeReq = message;
            blue.SendResponse();
        }

        private void OnArenaChallengeResponse(NetConnection<NetSession> sender, ArenaChallengeResponse message)
        {
            Log.InfoFormat("ArenaChallengeResponse::RedId:{0} RedName:{1} BlueID:{2} BlueName:{3}",
                message.ArenaInfo.Red.EntityId, message.ArenaInfo.Red.Name, message.ArenaInfo.Blue.EntityId, message.ArenaInfo.Blue.Name);
            var requester = SessionManager.Instance.GetSession(message.ArenaInfo.Red.EntityId);
            if (requester == null)
            {
                sender.Session.Response.arenaChallengeRes = message;
                sender.Session.Response.arenaChallengeRes.Result = Result.Failed;
                sender.Session.Response.arenaChallengeRes.Errormsg = "挑战者已下线";
                sender.SendResponse();
                return;
            }
            if (message.Result == Result.Failed)
            {
                requester.Session.Response.arenaChallengeRes = message;
                requester.Session.Response.arenaChallengeRes.Result = Result.Failed;
                requester.SendResponse();
                return;
            }
            //var arena = ArenaManager.Instance.NewArena(message.ArenaInfo, requester, sender);
            this.SendArenaBegin(requester,sender);
        }
        private void SendArenaBegin(NetConnection<NetSession> red, NetConnection<NetSession> blue)
        {
            var arenaBegin = new ArenaBeginResponse();
            arenaBegin.Result = Result.Failed;
            arenaBegin.Errormsg = "对方不在线";
            //arenaBegin.ArenaInfo = arena.ArenaInfo;
            red.Session.Response.arenaBegin = arenaBegin;
            red.SendResponse();
            blue.Session.Response.arenaBegin = arenaBegin;
            blue.SendResponse();
        }
    }
}
