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
	SkillDefine skill;

	float overlaySpeed = 0;
	float cdRemain = 0;

    void Start() { }
	void Update()
	{
		if (overlay.fillAmount > 0)
		{
			overlay.fillAmount = this.cdRemain / this.skill.CD;
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
		if(cdRemain > 0)
        {
			MessageBox.Show(string.Format("技能【{0}】正在冷却", this.skill.Name));
        }
        else
        {
			MessageBox.Show(string.Format("释放技能【{0}】", this.skill.Name));
			this.SetCD(this.skill.CD);
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
	}
	public void SetSkill(SkillDefine value)
    {
		this.skill = value;
		if(this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.skill.Icon);
		this.SetCD(this.skill.CD);
    }
}
