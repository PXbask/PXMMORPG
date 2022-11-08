using Common.Battle;
using Common.Data;
using Entities;
using Manager;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Battle
{
    public class Skill
    {
        public NSkillInfo Info;
        public Creature owner;
        public Creature Target;
        public SkillDefine Define;
        public NVector3 TargetPosition;
        private float cd = 0;

        public float CD
        {
            get { return cd; }
        }

        public SkillStatus Status { get; private set; }
        private Dictionary<int, List<NDamageInfo>> HitMap = new Dictionary<int, List<NDamageInfo>>();

        private float castingTime = 0;
        private float skillTime;
        public bool IsCasting = false;
        public int Hit = 0;
        private List<Bullet> Bullets = new List<Bullet>();

        public Skill(NSkillInfo info, Creature owner)
        {
            this.Info = info;
            this.owner = owner;
            this.Define = DataManager.Instance.Skills[(int)this.owner.Define.Class][this.Info.Id];
            this.cd = 0;
        }

        internal SkillResult CanCast(Creature target)
        {
            if (Define.CastTarget.Equals(SkillTarget.Target))
            {
                if (target == null || target == this.owner)
                    return SkillResult.InvalidTarget;
                int distance = this.owner.Distance(target);
                if (distance > this.Define.CastRange)
                    return SkillResult.OutOfRange;
            }
            if (Define.CastTarget.Equals(SkillTarget.Position) && BattleManager.Instance.CurrentPosition == null)
            {
                return SkillResult.InvalidTarget;
            }
            if (this.cd > 0)
            {
                return SkillResult.UnderCooling;
            }
            if(User.Instance.CurrentCharacter.Attributes.MP < Define.MPCost)
            {
                return SkillResult.InsufficientMp;
            }
            return SkillResult.Ok;
        }

        public void OnUpdate(float delta)
        {
            UpdateCD(delta);
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
                this.castingTime += Time.deltaTime;
            }
            else
            {
                this.castingTime = 0;
                this.StartRunningSkill();
                Debug.LogFormat("Skill[{0}].UpdateCasting Finished", this.Define.Name);
            }
        }
        private void UpdateSkill()
        {
            this.skillTime += Time.deltaTime;
            if (this.Define.Duration > 0)
            {
                if (this.skillTime >= this.Define.Interval * (this.Hit + 1))
                {
                    this.DoHit();
                }
                if (this.skillTime > this.Define.Duration)
                {
                    this.Status = SkillStatus.None;
                    this.IsCasting = false;
                    Debug.LogFormat("Skill[{0}].UpdateSkill Finished", this.Define.Name);
                }
            }
            else if (this.Define.HitTimes != null && this.Define.HitTimes.Count > 0)
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
                        Debug.LogFormat("Skill[{0}].UpdateSkill Finished", this.Define.Name);
                    }
                }
            }
            if (this.Define.Bullet)
            {
                bool finish = true;
                foreach (var bullet in this.Bullets)
                {
                    bullet.Update();
                    if (!bullet.Stopped) finish = false;
                }

                if (finish && this.Define.HitTimes.Count <= this.Hit)
                {
                    this.Status = SkillStatus.None;
                    Debug.LogFormat("Skill[{0}].UpdateSkill Finished", this.Define.Name);
                }
            }
        }
        private void DoHit()
        {
            if (this.Define.Bullet)
            {
                this.CastBullet();
            }
            else
            {
                this.DoHitDamages(this.Hit);
            }
            this.Hit++;
        }

        private void CastBullet()
        {
            Debug.LogFormat("Skill[{0}].CastBullet:[{1}]", Define.Name, Define.BulletResource);
            Bullet bullet = new Bullet(this);
            this.Bullets.Add(bullet);
            this.owner.PlayEffect(EffectType.Bullet, this.Define.BulletResource, this.Target, bullet.duration);
        }

        public void DoHitDamages(int hit)
        {
            List<NDamageInfo> damages = null;
            if (this.HitMap.TryGetValue(hit, out damages))
            {
                DoHitDamages(damages);
            }
        }
        private void DoHitDamages(List<NDamageInfo> damages)
        {
            foreach (var damage in damages)
            {
                Creature target = EntityManager.Instance.GetEntity(damage.entityID) as Creature;
                if (target == null) continue;
                target.DoDamage(damage, true);
                if(this.Define.HitEffect != null)
                {
                    target.PlayEffect(EffectType.Hit, this.Define.HitEffect, target);
                }
            }
        }

        public void DoHit(int hitId,List<NDamageInfo> damages)
        {
            if (hitId > this.Hit)
                this.HitMap[hitId] = damages;
            else
                DoHitDamages(damages);
        }
        private void UpdateCD(float delta)
        {
            if (cd > 0)
                cd -= delta;
            if (cd < 0)
                cd = 0;
        }
        public void DoHit(NSkillHitInfo hit)
        {
            if (hit.isBullet || !this.Define.Bullet)
            {
                this.DoHit(hit.hitId, hit.Damages);
            }
        }
        public void BeginCast(Creature target,NVector3 pos)
        {
            this.IsCasting = true;
            this.castingTime = 0;
            this.skillTime = 0;
            this.Hit = 0;
            this.cd = this.Define.CD;
            this.Target = target;
            this.TargetPosition = pos;
            this.owner.PlayAnim(Define.SkillAnim);
            this.Bullets.Clear();
            this.HitMap.Clear();

            if (this.Define.CastTarget.Equals(SkillTarget.Position))
            {
                this.owner.FaceTo(this.TargetPosition.ToVector3Int());
            }else if (this.Define.CastTarget.Equals(SkillTarget.Target))
            {
                this.owner.FaceTo(this.Target.position);
            }

            if (Define.CastTime > 0)
            {
                this.Status = SkillStatus.Casting;
            }
            else
            {
                StartRunningSkill();
            }
        }
        /// <summary>
        /// 技能执行开始 
        /// </summary>
        private void StartRunningSkill()
        {
            this.Status = SkillStatus.Running;
            if (!string.IsNullOrEmpty(this.Define.AOEEffect))
            {
                if (this.Define.CastTarget.Equals(SkillTarget.Position))
                {
                    this.owner.PlayEffect(EffectType.Position, this.Define.AOEEffect, this.TargetPosition);
                }else if (this.Define.CastTarget.Equals(SkillTarget.Target))
                {
                    this.owner.PlayEffect(EffectType.Position, this.Define.AOEEffect, this.Target);
                }
                else if (this.Define.CastTarget.Equals(SkillTarget.Self))
                {
                    this.owner.PlayEffect(EffectType.Position, this.Define.AOEEffect, this.owner);
                }
            }
        }
    }
}


