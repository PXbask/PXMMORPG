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
    internal class BattleService:Singleton<BattleService>
    {
        public BattleService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<SkillCastRequest>(this.SkillCast);
        }

        public void Init() { }
        private void SkillCast(NetConnection<NetSession> sender, SkillCastRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("SendCastSkill: id:{0} entityId:{1} targetId:{2} position:{3}", message.castInfo.skillId, message.castInfo.casterId, message.castInfo.targetId, message.castInfo.Position.String());
            BattleManager.Instance.ProcessBattleMessage(sender, message);
        }
    }
}
