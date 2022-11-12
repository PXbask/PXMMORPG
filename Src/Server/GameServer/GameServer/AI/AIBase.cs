using Common.Battle;
using Common.Data;
using GameServer.Battle;
using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.AI
{
    class AIBase
    {
        private Monster Owner;
        private Creature Target;
        private Skill normalSkill;
        public AIBase(Monster monster)
        {
            this.Owner= monster;
            this.normalSkill = this.Owner.SkillMgr.NormalSkill;
        }

        internal void Update()
        {
            if (this.Owner.BattleState.Equals(BattleState.InBattle))
            {
                this.UpdateBattle();
            }
        }

        private void UpdateBattle()
        {
            if (this.Target == null)
            {
                this.Owner.BattleState = BattleState.Idle;
                return;
            }
            if (!this.TryCastSkill())
            {
                if (!this.TryCastNormal())
                {
                    this.FollowTarget();
                }
            }
        }

        private bool TryCastSkill()
        {
            if(this.Target != null)
            {
                BattleContext context = new BattleContext(this.Owner.Map.Battle)
                {
                    Target = this.Target,
                    Caster = this.Owner,
                };
                Skill skill = this.Owner.FindSkill(context, SkillType.Skill);
                if (skill != null)
                {
                    this.Owner.CastSkill(context, skill.Define.Id);
                    return true;
                }
            }
            return false;
        }

        private bool TryCastNormal()
        {
            if (this.Target != null)
            {
                BattleContext context = new BattleContext(this.Owner.Map.Battle)
                {
                    Target = this.Target,
                    Caster = this.Owner,
                };
                var res = this.normalSkill.CanCast(context);
                if (res.Equals(SkillResult.Ok))
                {
                    this.Owner.CastSkill(context, normalSkill.Define.Id);
                }
                if(res.Equals(SkillResult.OutOfRange)) { return false; }
            }
            return true;
        }

        private void FollowTarget()
        {
            int distance = this.Owner.Distance(this.Target);
            if (distance > this.normalSkill.Define.CastRange - 50)
            {
                this.Owner.MoveTo(this.Target.Position);
            }
            else
            {
                this.Owner.StopMove();
            }
        }

        internal void OnDamage(NDamageInfo damage, Creature creature)
        {
            this.Target = creature;
        }
    }
}
