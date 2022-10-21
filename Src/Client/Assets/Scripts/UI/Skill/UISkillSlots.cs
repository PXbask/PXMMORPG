using Manager;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillSlots : MonoBehaviour {

	public UISkillSlot[] slots;
	// Use this for initialization
	void Start () {
		RefreshUI();
	}

    private void RefreshUI()
    {
        var Skills = DataManager.Instance.Skills[(int)User.Instance.CurrentCharacterInfo.Class];
        int index = 0;
        foreach (var skill in Skills)
        {
            slots[index++].SetSkill(skill.Value);
        }
    }
}
