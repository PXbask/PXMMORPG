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
        private Creature owner;
        public SkillDefine Define;
        public float CD;
        public Skill(NSkillInfo info, Creature owner)
        {
            this.Info = info;
            this.owner = owner;
            this.Define = DataManager.Instance.Skills[(int)this.owner.Define.Class][this.Info.Id];
        }

        internal SkillResult CanCast()
        {
            if(Define.CastTarget.Equals(SkillTarget.Target) && BattleManager.Instance.Target == null)
            {
                return SkillResult.InvalidTarget;
            }
            if(Define.CastTarget.Equals(SkillTarget.Position) && BattleManager.Instance.Position == Vector3.negativeInfinity)
            {
                return SkillResult.InvalidTarget;
            }
            if(this.CD > 0)
            {
                return SkillResult.UnderCooling;
            }
            if(User.Instance.CurrentCharacter.Attributes.MP < Define.MPCost)
            {
                return SkillResult.InsufficientMP;
            }
            return SkillResult.OK;
        }

        internal void Cast()
        {
            
        }
    }
}


