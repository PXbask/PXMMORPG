using Common.Data;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestSystem : UIWindow {
	public Text title;
	public GameObject itemPrefab;
	public UITabView tabs;
	public ListView listMain;
	public ListView listBrunch;
	public UIQuestInfo questInfo;
	private bool showAvailableList = false;

	protected override void OnStart()
    {
		this.listMain.onItemSelected += this.OnQuestSelected;
		this.listBrunch.onItemSelected += this.OnQuestSelected;
		this.tabs.OnTabSelect += this.OnSelectTab;
		this.RefreshUI();
        QuestManager.Instance.OnQuestChanged += this.RefreshUI;
    }
    private void OnDestroy()
    {
        QuestManager.Instance.OnQuestChanged -= this.RefreshUI;
    }
    private void RefreshUI()
    {
        this.ClearAllQuestList();
        this.InitAllQuestItems();
    }
    private void InitAllQuestItems()
    {
        foreach(var kv in QuestManager.Instance.allQuests)
        {
            if (showAvailableList)
            {
                if (kv.Value.Info != null) continue;
            }
            else
            {
                if (kv.Value.Info == null) continue;
            }
            GameObject go = Instantiate(itemPrefab, kv.Value.Define.Type == QuestType.Main ? listMain.transform : listBrunch.transform);
            UIQuestItem ui = go.GetComponent<UIQuestItem>();
            ui.SetQuestInfo(kv.Value);
            if (kv.Value.Define.Type == QuestType.Main)
                this.listMain.AddItem(ui as ListView.ListViewItem);
            else
                this.listBrunch.AddItem(ui as ListView.ListViewItem);
        }
    }

    private void ClearAllQuestList()
    {
        listMain.RemoveAll();
        listBrunch.RemoveAll();
    }
    private void OnSelectTab(int idx)
    {
        this.showAvailableList = idx == 1;
        this.RefreshUI();
    }

    private void OnQuestSelected(ListView.ListViewItem item)
    {
        UIQuestItem questItem = (UIQuestItem)item;
        this.questInfo.SetQuestInfo(questItem.quest);
    }
}
