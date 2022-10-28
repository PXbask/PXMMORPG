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
        public bool IsDeath = false;
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

            this.Attributes = new Attributes();
            this.Attributes.Init(this.Define, this.Info.Level, this.GetEquips(), Attributes.DynamicAttr);
            this.Info.attrDynamic = Attributes.DynamicAttr;
        }

        internal void DoDamage(NDamageInfo damage)
        {
            this.Attributes.HP -= damage.Damage;
            if(this.Attributes.HP < 0)
            {
                damage.willDead = true;
                this.IsDeath = true;
            }
        }

        private void InitSkills()
        {
            SkillMgr = new SkillManager(this);
            this.Info.Skills.AddRange(SkillMgr.Infos);
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
                skill.Cast(context);
        }

        public override void Update()
        {
            this.SkillMgr.Update();
        }
    }
}
