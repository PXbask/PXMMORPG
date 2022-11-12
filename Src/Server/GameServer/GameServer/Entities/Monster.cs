using Common;
using Common.Battle;
using Common.Data;
using GameServer.AI;
using GameServer.Battle;
using GameServer.Core;
using GameServer.Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GameServer.Entities
{
    class Monster : Creature
    {
        AIAgent AI;

        public Map Map;
        private Vector3Int moveTarget;
        private Vector3 movePosition;
        public Monster(int tid, int level, Vector3Int pos, Vector3Int dir) : base(CharacterType.Monster, tid, level, pos, dir)
        {
            this.AI = new AIAgent(this);
        }
        public void OnEnterMap(Map map)
        {
            this.Map= map;
        }
        protected override void OnDoDamage(NDamageInfo damage, Creature creature)
        {
            if(this.AI != null)
            {
                this.AI.OnDamage(damage, creature);
            }
        }
        public override void Update()
        {
            base.Update();
            this.UpdateMovement();
            this.AI.Update();
        }

        public Skill FindSkill(BattleContext context, SkillType type)
        {
            Skill skillcancast = null;
            foreach (var skill in this.SkillMgr.Skills)
            {
                if ((skill.Define.Type & type) != skill.Define.Type) continue;
                var res = skill.CanCast(context);
                if(res.Equals(SkillResult.Casting)) { return null; }
                if (res.Equals(SkillResult.Ok))
                {
                    skillcancast= skill;
                }
            }
            return skillcancast;
        }

        internal void MoveTo(Vector3Int position)
        {
            if(State.Equals(CharacterState.Idle))
            {
                State = CharacterState.Move;
            }
            if(this.moveTarget != position)
            {
                this.moveTarget = position;
                this.movePosition= this.Position;
                var dist = this.moveTarget - this.Position;
                this.Direction = dist.normalized;
                this.Speed = this.Define.Speed;
                Log.InfoFormat("Dir:{0} Speed:{1} Position:{2}",this.Direction.ToString(),this.Speed.ToString(),this.Position.ToString());
                NEntitySync sync = new NEntitySync();
                sync.Entity = this.EntityData;
                sync.Event = EntityEvent.MoveFwd;
                sync.Id = this.entityId;

                this.Map.UpdateEntity(sync);
            }
        }
        internal void StopMove()
        {
            this.State = CharacterState.Idle;
            this.moveTarget = Vector3Int.zero;
            this.Speed= 0;

            NEntitySync sync = new NEntitySync();
            sync.Entity = this.EntityData;
            sync.Event = EntityEvent.Idle;
            sync.Id = this.entityId;

            this.Map.UpdateEntity(sync);
        }
        private void UpdateMovement()
        {
            if (State == CharacterState.Move)
            {
                if(this.Distance(moveTarget) < 50) 
                {
                    this.StopMove();
                }
                if(this.Speed > 0)
                {
                    Vector3 dir = this.Direction;
                    this.movePosition += dir * this.Speed * TimeUtil.deltaTime / 100f;
                    this.Position = movePosition;
                }
            }
        }
    }
}
