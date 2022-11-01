using Common;
using Common.Data;
using Common.Utils;
using GameServer.Entities;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GameServer.Battle
{
    class Skill
    {
        private NSkillInfo Info;
        public Creature owner;
        public SkillDefine Define;
        private float cd;

        private float castingTime = 0;
        private float skillTime = 0;
        public int Hit = 0;
        private BattleContext Context; 
        private List<Bullet> Bullets = new List<Bullet>();

        public float CD
        {
            get { return cd; }
            set { cd = value; }
        }
        public bool Instant
        {
            get
            {
                if (Define.CastTime > 0) return false;
                if (Define.Bullet) return false;
                if (Define.Duration > 0) return false;
                if (this.Define.HitTimes != null && this.Define.HitTimes.Count > 0) return false;
                return true;
            }
        }
        public SkillStatus Status { get; private set; }

        public Skill(NSkillInfo info, Creature owner)
        {
            this.Info = info;
            this.owner = owner;
            this.Define = DataManager.Instance.Skills[(int)this.owner.Define.Class][this.Info.Id];
        }

        internal SkillResult Cast(BattleContext context)
        {
            SkillResult result = this.CanCast(context);
            
            if (result.Equals(SkillResult.Ok))
            {
                this.InitInfo(context);
                if (this.Instant)
                {
                    this.DoHit();
                }
                else
                {
                    if (Define.CastTime > 0)
                    {
                        this.Status = SkillStatus.Casting;
                    }
                    else
                    {
                        this.Status = SkillStatus.Running;
                    }
                }
                this.CD = this.Define.CD;
            }
            Log.InfoFormat("Skill:[{0}].Cast Result:[{1}] Status:{2}", this.Define.Name, result, this.Status);
            return result;
        }

        private void InitInfo(BattleContext context)
        {
            this.castingTime = 0;
            this.skillTime = 0;
            this.Hit = 0;
            this.cd = Define.CD;
            this.Bullets.Clear();
            this.Context = context;
        }
        NSkillHitInfo InitHitInfo(bool isBullet)
        {
            NSkillHitInfo HitInfo = new NSkillHitInfo();
            HitInfo.casterId = Context.Caster.entityId;
            HitInfo.skillId = this.Define.Id;
            HitInfo.hitId = this.Hit;
            HitInfo.isBullet = isBullet;
            return HitInfo;
        }
        public void DoHit()
        {
            NSkillHitInfo hitInfo = this.InitHitInfo(false);
            this.Hit++;
            Log.InfoFormat("Skill[{0}].DoHit:{1}", this.Define.Name, this.Hit);

            if (this.Define.Bullet)
            {
                CastBullet(hitInfo);
                return;
            }
            DoHit(hitInfo);
        }
        public void DoHit(NSkillHitInfo hitInfo)
        {
            this.Context.Battle.AddHitInfo(hitInfo);
            Log.InfoFormat("Skill[{0}].DoHit[{1}] IsBullet:{2}", Define.Name, hitInfo.hitId, hitInfo.isBullet);
            if (this.Define.AOERange > 0)
            {
                this.HitRange(hitInfo);
                return;
            }
            if (this.Define.CastTarget.Equals(SkillTarget.Target))
            {
                this.HitTarget(Context.Target, hitInfo);
            }
        }

        private void HitTarget(Creature target, NSkillHitInfo hitInfo)
        {
            if (this.Define.CastTarget.Equals(SkillTarget.Self) && target != Context.Caster) return;
            else if (target == Context.Caster) return;

            NDamageInfo damage = this.CalcSkillDamage(Context.Caster, target);
            Log.InfoFormat("Skill[{0}].HitTarget[{1}] Damage:{2} Crit:{3}", this.Define.Name, target.Name, damage.Damage, damage.Crit);
            target.DoDamage(damage);
            hitInfo.Damages.Add(damage);
        }

        private NDamageInfo CalcSkillDamage(Creature caster, Creature target)
        {
            float ad = this.Define.AD + caster.Attributes.AD * this.Define.ADFator;
            float ap = this.Define.AP + caster.Attributes.AP * this.Define.APFator;
            float addmg = ad * (1 - target.Attributes.DEF / (target.Attributes.DEF + 100));
            float apdmg = ap * (1 - target.Attributes.MDEF / (target.Attributes.MDEF + 100));
            float final = addmg + apdmg;
            bool isCrit = IsCrit(caster.Attributes.CRI);
            if (isCrit) final = final * 2f;
            //随即浮动
            final = final * ((float)MathUtil.Random.NextDouble() * 0.1f + 0.95f);
            NDamageInfo damage = new NDamageInfo();
            damage.entityID = target.entityId;
            damage.Damage = Math.Max(1, (int)final);
            damage.Crit = isCrit;
            return damage;
        }

        private bool IsCrit(float cRI)
        {
            return MathUtil.Random.NextDouble() < cRI;
        }

        private void HitRange(NSkillHitInfo hitInfo)
        {
            Core.Vector3Int pos;
            if (this.Define.CastTarget.Equals(SkillTarget.Target))
            {
                pos = Context.Target.Position;
            }
            else if (this.Define.CastTarget.Equals(SkillTarget.Position))
            {
                pos = Context.Position;
            }
            else
            {
                pos = Context.Caster.Position;
            }
            List<Creature> units = this.Context.Battle.FindUnitsInRange(pos, this.Define.AOERange);
            foreach (var target in units)
            {
                this.HitTarget(target, hitInfo);
            }
        }

        private void CastBullet(NSkillHitInfo hitInfo) 
        {
            this.Context.Battle.AddHitInfo(hitInfo);
            Log.InfoFormat("Skill[{0}].CastBullet:[{1}]", Define.Name, Define.BulletResource);
            Bullet bullet = new Bullet(this, this.Context.Target, hitInfo);
            this.Bullets.Add(bullet);
        }

        private SkillResult CanCast(BattleContext context)
        {
            if (this.Status != SkillStatus.None)
            {
                return SkillResult.Casting;
            }
            if (this.Define.CastTarget.Equals(SkillTarget.Target))
            {
                if(context.Target==null || context.Target == this.owner)
                {
                    return SkillResult.InvalidTarget;
                }
                int distance = this.owner.Distance(context.Target);
                if(distance > this.Define.CastRange)
                {
                    return SkillResult.OutOfRange;
                }
            }
            if (this.CD > 0)
                return SkillResult.UnderCooling;
            if (this.Define.CastTarget.Equals(SkillTarget.Position))
            {
                if (context.CastInfo.Position.Equals(null))
                {
                    return SkillResult.InvalidTarget;
                }
                if (this.owner.Distance(context.Position) > this.Define.CastRange)
                {
                    return SkillResult.OutOfRange;
                }
            }
            if (owner.Attributes.MP < Define.MPCost)
            {
                return SkillResult.InsufficientMp;
            }
            return SkillResult.Ok;
        }

        internal void Update()
        {
            UpdateCD();
            if (this.Status.Equals(SkillStatus.Casting))
            {
                this.UpdateCasting();
            }
            else if (this.Status.Equals(SkillStatus.Running))
            {
                this.UpdateSkill();
            }
        }

        private void UpdateCasting()
        {
            if (this.castingTime < this.Define.CastTime)
            {
                this.castingTime += TimeUtil.deltaTime;
            }
            else
            {
                this.castingTime = 0;
                this.Status = SkillStatus.Running;
                Log.InfoFormat("Skill[{0}].UpdateCasting Finished", this.Define.Name);
            }
        }

        private void UpdateCD()
        {
            if (cd > 0)
            {
                this.cd -= TimeUtil.deltaTime;
            }
            else
            {
                this.cd = 0;
            }
        }
        private void UpdateSkill()
        {
            this.skillTime += TimeUtil.deltaTime;
            if(this.Define.Duration > 0)
            {
                if (this.skillTime >= this.Define.Interval * (this.Hit + 1))
                {
                    this.DoHit();
                }
                if(this.skillTime > this.Define.Duration)
                {
                    this.Status = SkillStatus.None;
                    Log.InfoFormat("Skill[{0}].UpdateSkill Finished", this.Define.Name);
                }
            }
            else if(this.Define.HitTimes!=null && this.Define.HitTimes.Count > 0)
            {
                if (this.Hit < this.Define.HitTimes.Count)
                {
                    if (this.skillTime > this.Define.HitTimes[Hit])
                    {
                        this.DoHit();
                    }
                }
                else
                {
                    if (!this.Define.Bullet)
                    {
                        this.Status = SkillStatus.None;
                        Log.InfoFormat("Skill[{0}].UpdateSkill Finished", this.Define.Name);
                    }
                }
            }
            if (this.Define.Bullet)
            {
                bool finish = true;
                foreach (var bullet in this.Bullets)
                {
                    bullet.Update();
                    if (!bullet.isStopped) finish = false;
                }

                if (finish && this.Define.HitTimes.Count <= this.Hit)
                {
                    this.Status = SkillStatus.None;
                    Log.InfoFormat("Skill[{0}].UpdateSkill Finished", this.Define.Name);
                }
            }
        }
    }
}
