using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuild : UIWindow {

    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildMemberItem selectedItem;

    protected override void OnStart()
    {
        GuildService.Instance.OnGuildUpdate = UpdateUI;
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.UpdateUI();
    }
    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate = null;
        this.listMain.onItemSelected -= this.OnGuildMemberSelected;
    }
    private void UpdateUI()
    {
        this.uiInfo.Info = Manager.GuildManager.Instance.guildInfo;
        ClearList();
        InitItems();
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

    }
    public void OnClickLeave()
    {

    }
    public void OnClickChat()
    {

    }
    public void OnClickPromote()
    {

    }
    public void OnClickDepose()
    {

    }
    public void OnClickTransfer()
    {

    }
    public void OnClickSetNotice()
    {

    }
    #endregion

}
