using Common;
using Common.Data;
using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GameServer.Battle
{
    internal class Buff
    {
        private int buffID;
        private Creature owner;
        private BuffDefine define;
        private BattleContext context;
        public bool isStopped = false;
        private float time = 0;
        private int hit = 0;

        public Buff(int buffID, Creature owner, BuffDefine define, BattleContext context)
        {
            this.buffID = buffID;
            this.owner = owner;
            this.define = define;
            this.context = context;

            this.OnAdd();
        }

        private void OnAdd()
        {
            isStopped = false;
            this.AddEffect();
            this.AddAttr();
            NBuffInfo buff = new NBuffInfo()
            {
                buffId = this.buffID,
                buffType = this.define.Id,
                casterId = this.context.Caster.entityId,
                ownerId = this.owner.entityId,
                Action = BuffAction.Add,
            };
            context.Battle.AddBuffAction(buff);
        }
        private void OnRemove()
        {
            isStopped = true;
            this.RemoveEffect();
            this.RemoveAttr();
            NBuffInfo buff = new NBuffInfo()
            {
                buffId = this.buffID,
                buffType = this.define.Id,
                casterId = this.context.Caster.entityId,
                ownerId = this.owner.entityId,
                Action = BuffAction.Remove,
            };
            context.Battle.AddBuffAction(buff);
        }
        private void AddAttr()
        {
            if (this.define.DEFRatio != 0)
            {
                this.owner.Attributes.Buff.DEF += this.owner.Attributes.Basic.DEF * this.define.DEFRatio;
                this.owner.Attributes.InitFinalAttributes();
            }
        }
        private void RemoveAttr()
        {
            if (this.define.DEFRatio != 0)
            {
                this.owner.Attributes.Buff.DEF -= this.owner.Attributes.Basic.DEF * this.define.DEFRatio;
                this.owner.Attributes.InitFinalAttributes();
            }
        }

        private void AddEffect()
        {
            if (!this.define.Effect.Equals(BuffEffect.None))
            {
                this.owner.EffectMgr.AddEffect(this.define.Effect);
            }
        }
        private void RemoveEffect()
        {
            if (!this.define.Effect.Equals(BuffEffect.None))
            {
                this.owner.EffectMgr.RemoveEffect(this.define.Effect);
            }
        }
        public void Update()
        {
            if (isStopped) return;
            this.time += TimeUtil.deltaTime;
            if (this.define.Interval > 0)
            {
                if (this.time > this.define.Interval * (this.hit + 1))
                {
                    this.DoBuffDamage();
                }
            }
            if (time > this.define.Duration)
            {
                this.OnRemove();
            }
        }

        private void DoBuffDamage()
        {
            this.hit++;
            NDamageInfo damage = this.CalcBuffDamage(context.Caster);
            Log.InfoFormat("Buff[{0}].DoBuffDamage[{1}] Damage:{2} Crit:{3}",
                this.define.Name, this.owner.Name, damage.Damage, damage.Crit);
            this.owner.DoDamage(damage);
            NBuffInfo buff = new NBuffInfo()
            {
                buffId = this.buffID,
                buffType = this.define.Id,
                casterId = this.context.Caster.entityId,
                ownerId = this.owner.entityId,
                Action = BuffAction.Hit,
                Damage = damage,
            };
            context.Battle.AddBuffAction(buff);
        }

    private NDamageInfo CalcBuffDamage(Creature caster)
        {
            float ad = this.define.AD + caster.Attributes.AD * this.define.ADFator;
            float ap = this.define.AP + caster.Attributes.AP * this.define.APFator;
            float addmg = ad * (1 - this.owner.Attributes.DEF / (this.owner.Attributes.DEF + 100));
            float apdmg = ap * (1 - this.owner.Attributes.MDEF / (this.owner.Attributes.MDEF + 100));
            float final = addmg + apdmg;

            NDamageInfo damage = new NDamageInfo();
            damage.entityID = this.owner.entityId;
            damage.Damage = Math.Max(1, (int)final);
            return damage;
        }
    }
}
