using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuildList : UIWindow {
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildItem selectedItem;

    protected override void OnStart()
    {
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.uiInfo.Info = null;
        GuildService.Instance.OnGuildListResult += this.UpdateGuildList;
        GuildService.Instance.SendGuildListRequest();
    }
    private void OnDestroy()
    {
        GuildService.Instance.OnGuildListResult -= this.UpdateGuildList;
        this.listMain.onItemSelected -= this.OnGuildMemberSelected;
    }
    private void UpdateGuildList(List<NGuildInfo> guilds)
    {
        this.ClearList();
        InitItems(guilds);
    }
    private void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildItem;
        this.uiInfo.Info = this.selectedItem.Info;
    }
    private void InitItems(List<NGuildInfo> guilds)
    {
        foreach(var item in guilds)
        {
            GameObject go = Instantiate(itemPrefab, itemRoot);
            UIGuildItem itm = go.GetComponent<UIGuildItem>();
            itm.SetGuildInfo(item);
            listMain.AddItem(itm);
        }
    }

    private void ClearList()
    {
        this.listMain.RemoveAll();
    }

    public void OnClickJoin()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要加入的工会");
            return;
        }
        MessageBox.Show(String.Format("确定要加入公会【{0}】嘛？", selectedItem.Info.GuildName), "申请加入公会", MessageBoxType.Confirm, "确定", "取消")
            .OnYes = () => { GuildService.Instance.SendGuildJoinRequest(this.selectedItem.Info.Id); };
    }
}
