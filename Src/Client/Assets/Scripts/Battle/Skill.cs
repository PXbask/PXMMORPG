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
        private float castTime = 0;
        private float skillTime;
        public bool IsCasting = false;
        private int hit;

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
                int distance = (int)Vector3Int.Distance(this.owner.position, target.position);
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
            if (this.IsCasting)
            {
                this.skillTime += delta;
                if(this.skillTime > 0.5f && this.hit == 0)
                {
                    this.DoHit();
                }
                if(this.skillTime >= this.Define.CD)
                {
                    this.skillTime = 0;
                }
            }
            this.Update(delta);
        }

        private void DoHit()
        {
            if (this.Damage != null)
            {
                var cha = CharacterManager.Instance.GetCharacter(Damage.entityID);
                if (cha != null)
                {
                    cha.DoDamage(this.Damage);
                }
            }
            this.hit++;
        }

        private void Update(float delta)
        {
            if (cd > 0)
                cd -= delta;
            if (cd < 0)
                cd = 0;
        }

        public void BeginCast(NDamageInfo damage)
        {
            this.IsCasting = true;
            this.castTime = 0;
            this.skillTime = 0;
            this.hit = 0;
            this.cd = this.Define.CD;
            this.Damage = damage; 
            this.owner.PlayAnim(Define.SkillAnim);
        }
    }
}


