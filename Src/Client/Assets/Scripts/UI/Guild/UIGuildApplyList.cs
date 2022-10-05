using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using System;
using Manager;

public class UIGuildApplyList : UIWindow {

	public GameObject itemPrefab;
	public ListView listMain;
	public Transform itemRoot;

    protected override void OnStart()
    {
        GuildService.Instance.OnGuildUpdate += this.UpdateList;
        GuildService.Instance.SendGuildListRequest();
        UpdateList();
    }
    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= this.UpdateList;
    }

    private void UpdateList()
    {
        this.ClearList();
        this.InitItems();
    }

    private void InitItems()
    {
        foreach(var apply in GuildManager.Instance.guildInfo.Applies)
        {
            GameObject go = Instantiate(itemPrefab, itemRoot);
            UIGuildApplyItem item = go.GetComponent<UIGuildApplyItem>();
            item.SetItemInfo(apply);
            this.listMain.AddItem(item);
        }
    }

    private void ClearList()
    {
        listMain.RemoveAll();
    }
}
