using Entities;
using Manager;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Services
{
    internal class BattleService:Singleton<BattleService>
    {
        public BattleService()
        {
            MessageDistributer.Instance.Subscribe<SkillCastResponse>(this.OnSkillCast);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<SkillCastResponse>(this.OnSkillCast);
        }
        public void Init()
        {
            
        }
        private void OnSkillCast(object sender, SkillCastResponse message)
        {
            Debug.LogFormat("SendCastSkill: id:{0} entityId:{1} targetId:{2} position:{3}", message.castInfo.skillId, message.castInfo.casterId, message.castInfo.targetId, message.castInfo.Position.String());
            if (message.Result.Equals(SkillBridge.Message.Result.Success))
            {
                Creature caster = EntityManager.Instance.GetEntity(message.castInfo.casterId) as Creature;
                if(caster != null)
                {
                    Creature target = EntityManager.Instance.GetEntity(message.castInfo.targetId) as Creature;
                    caster.CastSkill(message.castInfo.skillId, target, message.castInfo.Position, message.Damage);
                }
            }
            else
            {
                ChatManager.Instance.AddSystemMessage(message.Errormsg);
            }
        }

        internal void SendSkillCast(int id, int entityId, int targetID, NVector3 position)
        {
            if (position == null) position = new NVector3();
            Debug.LogFormat("SendCastSkill: id:{0} entityId:{1} targetId:{2} position:{3}", id, entityId, targetID, position.String());
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.skillCast = new SkillCastRequest();
            message.Request.skillCast.castInfo = new NSkillCastInfo();
            message.Request.skillCast.castInfo.skillId = id;
            message.Request.skillCast.castInfo.casterId = entityId; 
            message.Request.skillCast.castInfo.targetId = targetID;
            message.Request.skillCast.castInfo.Position = position;
            NetClient.Instance.SendMessage(message);
        }
    }
}
