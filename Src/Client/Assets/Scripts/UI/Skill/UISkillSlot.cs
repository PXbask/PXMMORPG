using Battle;
using Common.Battle;
using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour,IPointerClickHandler {
	public Image icon;
	public Image overlay;
	public Text cdText;
	Skill skill;

	float overlaySpeed = 0;
	float cdRemain = 0;

    void Start() { }
	void Update()
	{
		if (overlay.fillAmount > 0)
		{
			overlay.fillAmount = this.cdRemain / this.skill.Define.CD;
			this.cdText.text = ((int)Math.Ceiling(cdRemain)).ToString();
			this.cdRemain -= Time.deltaTime;
        }
        else
        {
			if (overlay.enabled) overlay.enabled = false;
			if (cdText.enabled) cdText.enabled = false;
        }
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		SkillResult res = skill.CanCast();
        switch (res)
        {
			case SkillResult.InvalidTarget:
				MessageBox.Show("技能目标未指定");
				break;
			case SkillResult.InsufficientMP:
				MessageBox.Show("MP不足");
				break;
			case SkillResult.UnderCooling:
				MessageBox.Show(string.Format("技能【{0}】正在冷却", this.skill.Define.Name));
				break;
			case SkillResult.OK:
				MessageBox.Show(string.Format("释放技能【{0}】", this.skill.Define.Name));
				this.SetCD(this.skill.Define.CD);
				skill.Cast();
				break;
		}
	}

    private void SetCD(float cd)
    {
		if (!overlay.enabled) overlay.enabled = true;
		if (!cdText.enabled) cdText.enabled = true;
		this.cdText.text = ((int)Math.Ceiling(cdRemain)).ToString();
		overlay.fillAmount = 1;
		overlaySpeed = 1 / cd;
		cdRemain = cd;
		skill.CD = cd;
	}
	public void SetSkill(Skill value)
    {
		this.skill = value;
		if(this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.skill.Define.Icon);
		this.SetCD(this.skill.Define.CD);
    }
}
