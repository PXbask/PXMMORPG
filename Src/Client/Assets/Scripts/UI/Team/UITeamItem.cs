using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;

public class UITeamItem : ListView.ListViewItem {

	public Text nickName;
	public Image classIcon;
	public Image leaderIcon;
	public Image backGround;

    public override void onSelected(bool selected)
    {
        this.backGround.enabled = selected ? true : false;
    }
    public int index;
    public NCharacterInfo Info;

    void Start()
    {
        this.backGround.enabled = false;
    }
    public void SetMemberInfo(int index, NCharacterInfo item, bool isLeader)
    {
        this.index = index;
        this.Info = item;
        if (this.nickName != null) this.nickName.text = this.Info.Level.ToString().PadRight(4) + this.Info.Name;
        if (this.classIcon != null) this.classIcon.overrideSprite = SpriteManager.Instance.iconSprites[(int)this.Info.Class - 1];
        if (this.leaderIcon != null) this.leaderIcon.gameObject.SetActive(isLeader);
    }
}
