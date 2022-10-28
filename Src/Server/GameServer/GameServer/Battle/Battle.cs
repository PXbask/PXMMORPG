using GameServer.Entities;
using GameServer.Managers;
using GameServer.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    class Battle
    {
        public Map Map;
        Dictionary<int,Creature> AllUnits = new Dictionary<int,Creature>();
        Queue<NSkillCastInfo> Actions = new Queue<NSkillCastInfo>();
        List<Creature> DeathPool = new List<Creature>();
        public Battle(Map map)
        {
            this.Map = map;
        }
        internal void ProcessBattleMessage(NetConnection<NetSession> sender, SkillCastRequest request)
        {
            Character character = sender.Session.Character;
            if (request.castInfo != null)
            {
                if(request.castInfo.casterId != character.entityId)
                {
                    return;
                }
                this.Actions.Enqueue(request.castInfo);
            }
        }
        internal void Update()
        {
            if(this.Actions.Count > 0)
            {
                NSkillCastInfo castInfo = this.Actions.Dequeue();
                this.ExecuteAction(castInfo);
            }
            this.UpdateUnits();
        }

        private void ExecuteAction(NSkillCastInfo castInfo)
        {
            BattleContext context = new BattleContext(this);
            context.Caster = EntityManager.Instance.GetCreature(castInfo.casterId);
            context.Target = EntityManager.Instance.GetCreature(castInfo.targetId);
            context.CastInfo = castInfo;
            if (context.Caster != null)
            {
                this.JoinBattle(context.Caster);
            }
            if(context.Target != null)
            {
                this.JoinBattle(context.Target);
            }
            context.Caster.CastSkill(context, castInfo.skillId);

            NetMessageResponse message = new NetMessageResponse();
            message.skillCast = new SkillCastResponse();
            message.skillCast.castInfo = context.CastInfo;
            message.skillCast.Damage = context.Damage;
            message.skillCast.Result = context.Result.Equals(SkillResult.Ok) ? Result.Success : Result.Failed;
            message.skillCast.Errormsg = context.Result.ToString();
            this.Map.BroadcastBattleResponse(message);
        }
        public void JoinBattle(Creature creature)
        {
            if (!AllUnits.ContainsKey(creature.entityId))
                AllUnits.Add(creature.entityId, creature);
        }
        public void LeaveBattle(Creature creature)
        {
            AllUnits.Remove(creature.entityId);
        }
        private void UpdateUnits()
        {
            this.DeathPool.Clear();
            foreach (var kv in this.AllUnits)
            {
                kv.Value.Update();
                if(kv.Value.IsDeath)
                    this.DeathPool.Add(kv.Value);
            }
            foreach (var crea in this.DeathPool)
            {
                this.LeaveBattle(crea);
            }
        }
    }
}
