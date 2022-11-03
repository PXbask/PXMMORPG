﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;
using UnityEngine;
using Manager;
using Common.Data;
using Common.Battle;
using Battle;

namespace Entities
{
    public class Creature: Entity
    {
        public NCharacterInfo Info;

        public CharacterDefine Define;
        public Attributes Attributes; 
        public SkillManager SkillMgr;

        private bool battleState = false;
        public bool BattleState
        {
            get { return battleState; }
            set
            {
                if (value != battleState)
                {
                    battleState = value;
                    this.SetStandBy(value);
                }
            }
        }
        public Skill CastingSkill = null;
        public int ID { get { return this.Info.Id; } }
        public string Name
        {
            get
            {
                if (this.Info.Type == CharacterType.Player)
                    return this.Info.Name;
                else
                    return this.Define.Name;
            }
        }

        public bool IsPlayer
        {
            get { return this.Info.Type == CharacterType.Player; }
        }
        public bool isCurrentPlayer
        {
            get
            {
                if (!IsPlayer) return false;
                return this.Info.Id == Models.User.Instance.CurrentCharacterInfo.Id;
            }
        }

        public Creature(NCharacterInfo info) : base(info.Entity)
        {
            this.Info = info;
            this.Define = DataManager.Instance.Characters[info.ConfigId];
            this.Attributes=new Attributes();
            this.Attributes.Init(this.Define, this.Info.Level, this.GetEquips(), this.Info.attrDynamic);
            this.SkillMgr = new SkillManager(this);
        }
        public void UpdateCharacterInfo(NCharacterInfo info)
        {
            this.SetEntityData(info.Entity);
            this.Info = info;
            this.Attributes.Init(this.Define, this.Info.Level, this.GetEquips(), this.Info.attrDynamic);
            this.SkillMgr.UpdateSkills();
        }
        public virtual List<EquipDefine> GetEquips()
        {
            return null;
        }

        public void DoSkillHit(NSkillHitInfo info)
        {
            Skill skill = this.SkillMgr.GetSkill(info.skillId);
            skill.DoHit(info);
        }

        public void MoveForward()
        {
            Debug.LogFormat("MoveForward");
            this.speed = this.Define.Speed;
        }

        public void MoveBack()
        {
            Debug.LogFormat("MoveBack");
            this.speed = -this.Define.Speed;
        }

        public void Stop()
        {
            Debug.LogFormat("Stop");
            this.speed = 0;
        }

        public void SetDirection(Vector3Int direction)
        {
            Debug.LogFormat("SetDirection:{0}", direction);
            this.direction = direction;
        }

        public void SetPosition(Vector3Int position)
        {
            Debug.LogFormat("SetPosition:{0}", position);
            this.position = position;
        }
        public void CastSkill(int skillId, Creature target, NVector3 position)
        {
            this.SetStandBy(true);
            Skill skill = this.SkillMgr.GetSkill(skillId);
            skill.BeginCast(target);
        }
        public void PlayAnim(string Name)
        {
            if(this.Controller != null)
            {
                this.Controller.PlayAnim(Name);
            }
        }
        public void SetStandBy(bool standby)
        {
            if (this.Controller != null)
            {
                this.Controller.SetStandBy(standby);
            }
        }
        public override void UpdateInfo(float delta)
        {
            base.UpdateInfo(delta);
            this.SkillMgr.OnUpdate(delta);
        }
        public void DoDamage(NDamageInfo damage)
        {
            Debug.LogFormat("Creature:{0} Damage:{1} Crit:{2}", this.Name, damage.Damage, damage.Crit);
            this.Attributes.HP -= damage.Damage;
            this.PlayAnim("Hurt");
        }
        public int Distance(Creature target)
        {
            return (int)Vector3Int.Distance(position, target.position);
        }
        public int Distance(Vector3Int target)
        {
            return (int)Vector3Int.Distance(position, target);
        }
    }
}
