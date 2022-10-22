using Common.Data;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities {
    public class Character : Creature
    {
        public Character(NCharacterInfo info) : base(info)
        {
        }
        public override List<EquipDefine> GetEquips()
        {
            return Manager.EquipManager.Instance.GetEquipedDefines();
        }
    }
}


