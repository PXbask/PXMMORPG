using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildItem : ListView.ListViewItem {

    internal NGuildInfo Info;
	public Text ID;
	public Text Name;
	public Text Count;
	public Text Leader;

	public Image background;
	public Sprite normalBg;
	public Sprite selectedBg;

	public override void onSelected(bool selected)
	{
		this.background.overrideSprite = selected ? selectedBg : normalBg;
	}
	internal void SetGuildInfo(NGuildInfo item)
    {
		this.Info = item;
		if (this.ID != null) this.ID.text = this.Info.Id.ToString();
		if (this.Name != null) this.Name.text = this.Info.GuildName;
		if (this.Count != null) this.Count.text = this.Info.memberCount.ToString();
		if (this.Leader != null) this.Leader.text = this.Info.leaderName;

	}
}
