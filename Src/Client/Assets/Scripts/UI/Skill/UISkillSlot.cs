using Battle;
using Common.Battle;
using Common.Data;
using Manager;
using SkillBridge.Message;
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

	void Start()
	{
		overlay.enabled = false;
		cdText.enabled = false;
	}
	void Update()
	{
		if (this.skill == null) return;
		if (this.skill.CD > 0)
		{
			if (!overlay.enabled) overlay.enabled = true;
			if (!cdText.enabled) cdText.enabled = true;
			overlay.fillAmount = this.skill.CD / this.skill.Define.CD;
			cdText.text = ((int)Math.Ceiling(this.skill.CD)).ToString(); 
		}
        else
        {
			if (overlay.enabled) overlay.enabled = false;
			if (cdText.enabled) cdText.enabled = false;
        }
	}
	private void OnPositionSelected(Vector3 pos)
    {
		BattleManager.Instance.CurrentPosition = GameObjectTool.WorldToLogicN(pos);
		this.CastSkill();
    }
	public void OnPointerClick(PointerEventData eventData)
	{
        if (skill.Define.CastTarget.Equals(SkillTarget.Position))
        {
			TargetSelector.ShowSelector(this.skill.Define.CastRange, this.skill.Define.AOERange, this.OnPositionSelected);
			return;
        }
		CastSkill();
	}
	private void CastSkill()
	{
		SkillResult res = skill.CanCast(BattleManager.Instance.CurrentTarget);
        switch (res)
        {
			case SkillResult.InvalidTarget:
				MessageBox.Show("技能目标未指定");
				break;
			case SkillResult.InsufficientMp:
				MessageBox.Show("MP不足");
				break;
			case SkillResult.UnderCooling:
				MessageBox.Show(string.Format("技能【{0}】正在冷却", this.skill.Define.Name));
				break;
			case SkillResult.OutOfRange:
				MessageBox.Show("目标超出技能范围");
				break;
			case SkillResult.Ok:
				BattleManager.Instance.CastSkill(this.skill);
				break;
		}
	}

	public void SetSkill(Skill value)
    {
		this.skill = value;
		if (this.icon != null)
		{
			this.icon.overrideSprite = Resloader.Load<Sprite>(this.skill.Define.Icon);
			this.icon.SetAllDirty();
		}
    }
}
