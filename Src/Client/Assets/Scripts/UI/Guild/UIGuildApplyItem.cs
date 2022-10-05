using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildApplyItem : ListView.ListViewItem {

    private NGuildApplyInfo Info;

    public Text nickName;
    public Text @class;
    public Text level;

    internal void SetItemInfo(NGuildApplyInfo apply)
    {
        this.Info = apply;
        if(this.nickName != null) this.nickName.text = this.Info.Name;
        if(this.@class != null) this.@class.text = this.Info.Class.ToString();
        if(this.level != null) this.level.text = this.Info.Level.ToString();
    }
    public void OnAccept()
    {
        MessageBox.Show(String.Format("要通过【{0}】的公会申请嘛?", this.Info.Name), "审批申请", MessageBoxType.Confirm, "确定", "取消")
            .OnYes = () => { GuildService.Instance.SendGuildJoinApply(true,this.Info); };
    }
    public void OnDecline()
    {
        MessageBox.Show(String.Format("要拒绝【{0}】的公会申请嘛?", this.Info.Name), "审批申请", MessageBoxType.Confirm, "确定", "取消")
            .OnYes = () => { GuildService.Instance.SendGuildJoinApply(false, this.Info); };
    }
}
