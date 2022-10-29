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
        public SkillDefine Define;
        private float cd = 0;
        public NDamageInfo Damage { get; private set; }

        public float CD
        {
            get { return cd; }
        }

        public SkillStatus Status { get; private set; }
        private Dictionary<int, List<NDamageInfo>> HitMap = new Dictionary<int, List<NDamageInfo>>();

        private float castingTime = 0;
        private float skillTime;
        public bool IsCasting = false;
        private int Hit;

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
                this.Status = SkillStatus.Running;
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
                    this.Status = SkillStatus.None;
                    this.IsCasting = false;
                    Debug.LogFormat("Skill[{0}].UpdateSkill Finished", this.Define.Name);
                }
            }
        }
        private void DoHit()
        {
            List<NDamageInfo> damages = null;
            if(this.HitMap.TryGetValue(this.Hit,out damages))
            {
                DoHitDamages(damages);
            }
            this.Hit++;
        }

        private void DoHitDamages(List<NDamageInfo> damages)
        {
            foreach (var damage in damages)
            {
                Creature target = EntityManager.Instance.GetEntity(damage.entityID) as Creature;
                if (target == null) continue;
                target.DoDamage(damage);
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

        public void BeginCast(NDamageInfo damage)
        {
            this.IsCasting = true;
            this.castingTime = 0;
            this.skillTime = 0;
            this.Hit = 0;
            this.cd = this.Define.CD;
            this.Damage = damage; 
            this.owner.PlayAnim(Define.SkillAnim);
            if (Define.CastTime > 0)
            {
                this.Status = SkillStatus.Casting;
            }
            else
            {
                this.Status = SkillStatus.Running;
            }
        }
    }
}


