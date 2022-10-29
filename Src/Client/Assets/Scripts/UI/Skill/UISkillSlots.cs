using Manager;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillSlots : MonoBehaviour {

	public UISkillSlot[] slots;

	void Start () {
	}

    public void RefreshUI()
    {
        if (User.Instance.CurrentCharacter == null) return;
        var Skills = User.Instance.CurrentCharacter.SkillMgr.Skills;
        int index = 0;
        foreach (var skill in Skills)
        {
            slots[index++].SetSkill(skill);
        }
    }
}
