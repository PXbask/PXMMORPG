using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildInfo : MonoBehaviour {

	public Text guildName;
	public Text guildID;
	public Text leader;
	public Text notice;
	public Text memberCount;

	private SkillBridge.Message.NGuildInfo info;
	public SkillBridge.Message.NGuildInfo Info
    {
		get { return info; }
        set
        {
			info = value;
			this.UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if(this.info == null)
        {
            this.guildName.text = "无";
            this.guildID.text = "ID:0";
            this.leader.text = "无";
            this.notice.text = String.Empty;
            this.memberCount.text = String.Format("成员数量：0/{0}", GameDefine.GuildMaxMemberCount);
        }
        else
        {
            this.guildName.text = this.Info.GuildName;
            this.guildID.text = "ID:" + this.Info.Id;
            this.leader.text = this.Info.leaderName;
            this.notice.text = this.Info.Notice;
            this.memberCount.text = String.Format("成员数量：{0}/{1}", this.Info.memberCount, GameDefine.GuildMaxMemberCount);
        }
    }
}
