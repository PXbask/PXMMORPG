using Common.Battle;
using Common.Data;
using GameServer.Battle;
using GameServer.Core;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
        class Creature : Entity
        {

        public int Id { get; set; }
        public string Name { get { return Info.Name; } }
        public NCharacterInfo Info;
        public CharacterDefine Define;
        public Attributes Attributes;
        public SkillManager SkillMgr;
        public BuffManager BuffMgr;
        public EffectManager EffectMgr;
        public bool IsDeath = false;

        public CharState State;     
        public Creature(CharacterType type, int configId, int level, Vector3Int pos, Vector3Int dir) :base(pos, dir)
        {
            this.Define = DataManager.Instance.Characters[configId];
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Level = level;
            this.Info.ConfigId = configId;
            this.Info.Entity = this.EntityData;
            this.Info.EntityId = this.entityId;

            this.Info.Name = this.Define.Name;
            this.InitSkills();
            this.InitBuff();
            this.Attributes = new Attributes();
            this.Attributes.Init(this.Define, this.Info.Level, this.GetEquips(), Attributes.DynamicAttr);
            this.Info.attrDynamic = Attributes.DynamicAttr;
        }



        internal void DoDamage(NDamageInfo damage, Creature creature)
        {
            this.State = CharState.InBattle;
            this.Attributes.HP -= damage.Damage;
            if(this.Attributes.HP < 0)
            {
                damage.willDead = true;
                this.IsDeath = true;
            }
            this.OnDoDamage(damage, creature);
        }

        protected virtual void OnDoDamage(NDamageInfo damage, Creature creature)
        {
            
        }

        private void InitSkills()
        {
            SkillMgr = new SkillManager(this);
            this.Info.Skills.AddRange(SkillMgr.Infos);
        }
        private void InitBuff()
        {
            BuffMgr = new BuffManager(this);
            EffectMgr = new EffectManager(this);
        }
        public virtual List<EquipDefine> GetEquips()
        {
            return null;
        }

        public int Distance(Creature target)
        {
            return (int)Vector3Int.Distance(Position, target.Position);
        }
        public int Distance(Vector3Int target)
        {
            return (int)Vector3Int.Distance(Position, target);
        }

        public void CastSkill(BattleContext context, int skillId)
        {
            Skill skill = this.SkillMgr.GetSkill(skillId);
            if(skill != null)
                context.Result = skill.Cast(context);
            if (context.Result.Equals(SkillResult.Ok))
            {
                this.State = CharState.InBattle;
            }
            if (context.CastInfo == null)
            {
                if (context.Result.Equals(SkillResult.Ok))
                {
                    context.CastInfo = new NSkillCastInfo()
                    {
                        casterId = this.entityId,
                        targetId = context.Target.entityId,
                        skillId = skill.Define.Id,
                        Position = new NVector3(),
                        Result = context.Result,
                    };
                    context.Battle.AddCastSkillInfo(context.CastInfo);
                }
            }
            else
            {
                context.CastInfo.Result = context.Result;
                context.Battle.AddCastSkillInfo(context.CastInfo);
            }
        }

        public override void Update()
        {
            this.SkillMgr.Update();
            this.BuffMgr.Update();
        }

        internal void AddBuff(BattleContext context, BuffDefine buffDefine)
        {
            this.BuffMgr.AddBuff(context, buffDefine);
        }
    }
}
