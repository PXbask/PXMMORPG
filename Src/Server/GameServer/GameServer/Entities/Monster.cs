using Common.Battle;
using GameServer.Battle;
using GameServer.Core;
using GameServer.Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
    class Monster : Creature
    {
        public Creature Target;
        private Map Map;
        public Monster(int tid, int level, Vector3Int pos, Vector3Int dir) : base(CharacterType.Monster, tid, level, pos, dir)
        {

        }
        public void OnEnterMap(Map map)
        {
            this.Map= map;
        }
        protected override void OnDoDamage(NDamageInfo damage, Creature creature)
        {
            if(this.Target == null)
            {
                this.Target = creature;
            }
        }
        public override void Update()
        {
            if(this.State.Equals(CharState.InBattle))
            {
                this.UpdateBattle();
            }
            base.Update();
        }

        private void UpdateBattle()
        {
            if (this.Target != null)
            {
                BattleContext context = new BattleContext(this.Map.Battle)
                {
                    Target = this.Target,
                    Caster = this,
                };
                Skill skill = this.FindSkill(context);
                if (skill != null)
                {
                    this.CastSkill(context, skill.Define.Id);
                }
            }
        }

        private Skill FindSkill(BattleContext context)
        {
            Skill skillcancast = null;
            foreach (var skill in this.SkillMgr.Skills)
            {
                var res = skill.CanCast(context);
                if(res.Equals(SkillResult.Casting)) { return null; }
                if (res.Equals(SkillResult.Ok))
                {
                    skillcancast= skill;
                }
            }
            return skillcancast;
        }
    }
}
