using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFriendItem : ListView.ListViewItem {
	public Text nickname;
	public Text @class;
	public Text level;
	public Text status;

	public Image background;
	public Sprite normalBg;
	public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
		this.background.overrideSprite = selected ? selectedBg : normalBg;
    }
	public NFriendInfo Info;

    public void SetFriendInfo(NFriendInfo info)
    {
		this.Info = info;
		if (this.nickname != null) this.nickname.text = info.friendInfo.Name;
		if (this.@class != null) this.@class.text = info.friendInfo.Class.ToString();
		if (this.level != null) this.level.text = info.friendInfo.Level.ToString();
		if (this.status != null) this.status.text = info.Status == 1 ? "OnLine" : "OutLine";
    }
}
