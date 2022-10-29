using GameServer.Core;
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
        List<NSkillHitInfo> Hits = new List<NSkillHitInfo>();
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
            this.Hits.Clear();
            if(this.Actions.Count > 0)
            {
                NSkillCastInfo castInfo = this.Actions.Dequeue();
                this.ExecuteAction(castInfo);
            }
            this.UpdateUnits();
            this.BroadcastHitMessages();
        }

        private void BroadcastHitMessages()
        {
            if (this.Hits.Count <= 0) return;
            NetMessageResponse message = new NetMessageResponse();
            message.skillHits = new SkillHitResponse();
            message.skillHits.Hits.AddRange(this.Hits);
            message.skillHits.Result = Result.Success;
            message.skillHits.Errormsg = "";
            this.Map.BroadcastBattleResponse(message);
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

            NetMessageResponse message = new NetMessageResponse();
            message.skillCast = new SkillCastResponse();
            message.skillCast.castInfo = context.CastInfo;
            message.skillCast.Damage = context.Damage;
            message.skillCast.Result = context.Result.Equals(SkillResult.Ok) ? Result.Success : Result.Failed;
            message.skillCast.Errormsg = context.Result.ToString();
            this.Map.BroadcastBattleResponse(message);

            context.Caster.CastSkill(context, castInfo.skillId);
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

        public void AddHitInfo(NSkillHitInfo hitInfo)
        {
            this.Hits.Add(hitInfo);
        }

        public List<Creature> FindUnitsInRange(Vector3Int pos, int range)
        {
            List<Creature> res = new List<Creature>();
            foreach (var kv in this.AllUnits)
            {
                if (kv.Value.Distance(pos) < range)
                {
                    res.Add(kv.Value);
                }
            }
            return res;
        }
    }
}
