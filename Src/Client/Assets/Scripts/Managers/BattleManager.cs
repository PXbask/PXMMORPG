using Battle;
using Entities;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Manager
{
    class BattleManager:Singleton<BattleManager>
    {
        public delegate void TargetChangedHandler(Creature creature);
        public event TargetChangedHandler OnTargetChanged;
        private Creature currentTarget;

        public Creature CurrentTarget
        {
            get { return currentTarget; }
            set { this.SetTarget(value); }
        }

        private void SetTarget(Creature value)
        {
            if(this.currentTarget!=value && this.OnTargetChanged != null)
            {
                this.OnTargetChanged(value);
            }
            currentTarget = value;
            Debug.LogFormat("BattleTarget: SetTarget:[{0}]{1}", value.ID, value.Name);
        }

        private NVector3 currentPosition;

        public NVector3 CurrentPosition
        {
            get { return currentPosition; }
            set { this.SetPosition(value); }
        }

        private void SetPosition(NVector3 value)
        {
            currentPosition = value;
            Debug.LogFormat("BattleTarget: SetPosition:[{0}]", value);
        }
        public void CastSkill(Skill skill)
        {
            int targetID = currentTarget != null ? currentTarget.ID : 0;
            BattleService.Instance.SendSkillCast(skill.Define.Id, skill.owner.entityId, targetID, currentPosition);
        }
    }
}
