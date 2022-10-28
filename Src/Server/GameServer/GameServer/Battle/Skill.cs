using Common.Data;
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
        private Creature owner;
        public SkillDefine Define;
        private float cd;

        public float CD
        {
            get { return cd; }
            set { cd = value; }
        }

        public Skill(NSkillInfo info, Creature owner)
        {
            this.Info = info;
            this.owner = owner;
            this.Define = DataManager.Instance.Skills[(int)this.owner.Define.Class][this.Info.Id];
        }

        internal SkillResult Cast(BattleContext context)
        {
            SkillResult result = SkillResult.Ok;
            if (this.cd > 0)
            {
                return SkillResult.UnderCooling;
            }
            if (context.Target != null)
            {
                this.DoSkillDamage(context);
            }
            this.cd = this.Define.CD;
            return result;
        }

        private void DoSkillDamage(BattleContext context)
        {
            context.Damage = new NDamageInfo();
            context.Damage.entityID = context.Target.entityId;
            context.Damage.Damage = 100;
            context.Target.DoDamage(context.Damage);
        }

        internal void Update()
        {
            UpdateCD();
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
    }
}
