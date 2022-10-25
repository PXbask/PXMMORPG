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
        public float CD
        {
            get { return cd; }
        }
        private float castTime = 0;
        public bool IsCasting = false;
        public Skill(NSkillInfo info, Creature owner)
        {
            this.Info = info;
            this.owner = owner;
            this.Define = DataManager.Instance.Skills[(int)this.owner.Define.Class][this.Info.Id];
            this.cd = 0;
        }

        internal SkillResult CanCast(Creature target)
        {
            //if(Define.CastTarget.Equals(SkillTarget.Target))
            //{
            //    if (target == null || target == this.owner)
            //        return SkillResult.InvalidTarget;
            //}
            //if(Define.CastTarget.Equals(SkillTarget.Position) && BattleManager.Instance.CurrentPosition == null)
            //{
            //    return SkillResult.InvalidTarget;
            //}
            if(this.cd > 0)
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
            if (cd > 0)
                cd -= delta;
            if (cd < 0)
                cd = 0;
        }

        public void BeginCast()
        {
            this.IsCasting = true;
            this.castTime = 0;
            this.cd = this.Define.CD;
            this.owner.PlayAnim(Define.SkillAnim);
        }
    }
}


