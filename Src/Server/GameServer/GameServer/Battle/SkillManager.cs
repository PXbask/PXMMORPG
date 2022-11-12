﻿using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    class SkillManager
    {
        Creature Owner;
        public List<Skill> Skills { get; private set; }
        public List<NSkillInfo> Infos { get; private set; }
        public Skill NormalSkill { get; set; }
        public SkillManager(Creature creatrue)
        {
            Owner = creatrue;
            Skills = new List<Skill>();
            Infos = new List<NSkillInfo>();
            this.InitSkills();
        }

        private void InitSkills()
        {
            this.Skills.Clear();
            this.Infos.Clear();

            if (!DataManager.Instance.Skills.ContainsKey(this.Owner.Define.TID)) return;

            foreach (var define in DataManager.Instance.Skills[this.Owner.Define.TID])
            {
                NSkillInfo info = new NSkillInfo();
                info.Id = define.Key;
                if (this.Owner.Info.Level >= define.Value.UnlockLevel)
                {
                    info.Level = 5;
                }
                else
                {
                    info.Level = 1;
                }
                this.Infos.Add(info);
                Skill skill = new Skill(info, this.Owner);
                if(define.Value.Type.Equals(SkillType.Normal)) { this.NormalSkill= skill; }
                this.AddSkill(skill);
            }
        }
        public void AddSkill(Skill skill)
        {
            this.Skills.Add(skill);
        }

        internal Skill GetSkill(int skillId)
        {
            foreach (var skill in this.Skills)
            {
                if(skill.Define.Id == skillId)
                    return skill;
            }
            return null;
        }

        internal void Update()
        {
            foreach (var skill in this.Skills)
            {
                skill.Update();
            }
        }
    }
}
