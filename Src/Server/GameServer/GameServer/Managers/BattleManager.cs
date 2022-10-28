using Common;
using GameServer.Entities;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    internal class BattleManager : Singleton<BattleManager>
    {
        private static long bid = 0;
        public void Init() { }
        public void ProcessBattleMessage(NetConnection<NetSession> sender, SkillCastRequest message)
        {
            Log.InfoFormat("BattleManager.ProcessBattleMessage: skill:{0} caster:{1} target:{2} pos:{3}",
                message.castInfo.skillId, message.castInfo.casterId, message.castInfo.targetId, message.castInfo.Position.String());
            Character character = sender.Session.Character;
            var battle = MapManager.Instance[character.Info.mapId].Battle;
            battle.ProcessBattleMessage(sender, message);
        }
    }
}
