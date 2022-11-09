﻿using GameServer.Core;
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
        List<NSkillCastInfo> CastSkills = new List<NSkillCastInfo>();
        List<Creature> DeathPool = new List<Creature>();
        List<NSkillHitInfo> Hits = new List<NSkillHitInfo>();
        List<NBuffInfo> BuffActions = new List<NBuffInfo>();
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
            this.CastSkills.Clear();
            this.Hits.Clear();
            this.BuffActions.Clear();
            if (this.Actions.Count > 0)
            {
                NSkillCastInfo castInfo = this.Actions.Dequeue();
                this.ExecuteAction(castInfo);
            }
            this.UpdateUnits();
            this.BroadcastHitMessages();
        }


        private void BroadcastHitMessages()
        {
            if (this.Hits.Count <= 0 && this.BuffActions.Count <= 0 && this.CastSkills.Count <= 0) return;
            NetMessageResponse message = new NetMessageResponse();
            if (this.CastSkills.Count > 0)
            {
                message.skillCast = new SkillCastResponse();
                message.skillCast.castInfoes.AddRange(this.CastSkills);
                message.skillCast.Result = Result.Success;
                message.skillCast.Errormsg = "";
            }
            if (this.Hits.Count > 0)
            {
                message.skillHits = new SkillHitResponse();
                message.skillHits.Hits.AddRange(this.Hits);
                message.skillHits.Result = Result.Success;
                message.skillHits.Errormsg = "";
            }
            if (this.BuffActions.Count > 0)
            {
                message.buffRes = new BuffResponse();
                message.buffRes.Buffs.AddRange(this.BuffActions);
                message.buffRes.Result = Result.Success;
                message.buffRes.Errormsg = "";
            }
            this.Map.BroadcastBattleResponse(message);
        }

        private void ExecuteAction(NSkillCastInfo castInfo)
        {
            BattleContext context = new BattleContext(this);
            context.Caster = EntityManager.Instance.GetCreature(castInfo.casterId);
            context.Target = EntityManager.Instance.GetCreature(castInfo.targetId);
            context.Position = castInfo.Position;
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
        public void AddCastSkillInfo(NSkillCastInfo castInfo)
        {
            this.CastSkills.Add(castInfo);
        }
        public void AddHitInfo(NSkillHitInfo hitInfo)
        {
            this.Hits.Add(hitInfo);
        }
        public void AddBuffAction(NBuffInfo buff)
        {
            this.BuffActions.Add(buff);
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
        public List<Creature> FindUnitsInMapRange(Vector3Int pos,int range)
        {
            return EntityManager.Instance.GetMapEntitiesInRange<Creature>(this.Map.ID, pos, range);
        }
    }
}
