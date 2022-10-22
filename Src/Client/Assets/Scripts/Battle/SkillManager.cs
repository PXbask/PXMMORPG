using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
	public class SkillManager
	{
		Creature Owner;
		public List<Skill> Skills { get; private set; }
		public SkillManager(Creature creatrue)
        {
			Owner = creatrue;
			Skills = new List<Skill>();
			this.InitSkills();
        }

        private void InitSkills()
        {
            this.Skills.Clear();
			foreach(var skillInfo in this.Owner.Info.Skills)
            {
                Skill skill = new Skill(skillInfo, this.Owner);
                this.AddSkill(skill);
            }
        }
        public void AddSkill(Skill skill)
        {
            this.Skills.Add(skill);
        }
    }
}

