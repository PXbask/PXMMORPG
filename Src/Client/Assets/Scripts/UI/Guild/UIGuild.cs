using Manager;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;

public class UIGuild : UIWindow {

    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildMemberItem selectedItem;

    public GameObject panelAdmin;
    public GameObject panelLeader;

    protected override void OnStart()
    {
        GuildService.Instance.OnGuildUpdate += UpdateUI;
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.UpdateUI();
    }
    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateUI;
        this.listMain.onItemSelected -= this.OnGuildMemberSelected;
    }
    private void UpdateUI()
    {
        this.uiInfo.Info = Manager.GuildManager.Instance.guildInfo;
        ClearList();
        InitItems();
        ResetButtons();
    }

    private void ResetButtons()
    {
        panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title > SkillBridge.Message.GuildTitle.None);
        panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title == SkillBridge.Message.GuildTitle.President);
    }

    private void InitItems()
    {
        foreach (var item in Manager.GuildManager.Instance.guildInfo.Members)
        {
            GameObject go = Instantiate(itemPrefab, itemRoot);
            UIGuildMemberItem itm = go.GetComponent<UIGuildMemberItem>();
            itm.SetGuildMemberInfo(item);
            listMain.AddItem(itm);
        }
    }

    private void ClearList()
    {
        this.listMain.RemoveAll();
    }

    private void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildMemberItem;
    }
    #region Event
    public void OnClickAppliesList()
    {
        UIManager.Instance.Show<UIGuildApplyList>();
    }
    public void OnClickLeave()
    {
        MessageBox.Show("扩展作业");
    }
    public void OnClickChat()
    {

    }
    public void OnClickKickout()
    {
        if(selectedItem == null)
        {
            MessageBox.Show("请选择要踢出的成员");
            return;
        }
        MessageBox.Show(String.Format("要踢【{0}】出公会嘛?", this.selectedItem.Info.Info.Name), "踢出公会", MessageBoxType.Confirm,"确定", "取消")
            .OnYes = () => { GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, this.selectedItem.Info.Info.Id); };
    }
    public void OnClickPromote()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要晋升的成员");
            return;
        }
        if (selectedItem.Info.Title != GuildTitle.None)
        {
            MessageBox.Show("对方已无法继续晋升");
            return;
        }
        MessageBox.Show(String.Format("要晋升【{0}】为副会长嘛?", this.selectedItem.Info.Info.Name), "成员晋升", MessageBoxType.Confirm, "确定", "取消")
           .OnYes = () => { GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedItem.Info.Info.Id); };
    }
    public void OnClickDepose()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要罢免的成员");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.None)
        {
            MessageBox.Show("对方已无法继续罢免");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.President)
        {
            MessageBox.Show("会长无法罢免");
            return;
        }
        MessageBox.Show(String.Format("要罢免【{0}】职务吗嘛?", this.selectedItem.Info.Info.Name), "成员晋升", MessageBoxType.Confirm, "确定", "取消")
           .OnYes = () => { GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, this.selectedItem.Info.Info.Id); };
    }
    public void OnClickTransfer()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要转让的成员");
            return;
        }
        MessageBox.Show(String.Format("要把会长转让给【{0}】嘛?", this.selectedItem.Info.Info.Name), "成员晋升", MessageBoxType.Confirm, "确定", "取消")
       .OnYes = () => { GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedItem.Info.Info.Id); };
    }
    public void OnClickSetNotice()
    {
        MessageBox.Show("扩展作业");
    }
    #endregion

}
