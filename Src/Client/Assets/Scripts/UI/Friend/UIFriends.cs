using Manager;
using Models;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFriends : UIWindow {

	public GameObject itemPrefab;
	public ListView listMain;
	public Transform itemRoot;
	public UIFriendItem selectedItem;

	protected override void OnStart()
    {
		FriendService.Instance.OnFriendUpdate += RefreshUI;
		this.listMain.onItemSelected += this.OnFriendSelected;
		RefreshUI();

	}
    private void Update() { }
	private void OnFriendSelected(ListView.ListViewItem item)
	{
		this.selectedItem=item as UIFriendItem;
	}
	public void OnClickFriendAdd()
    {
		InputBox.Show("输入想添加的好友名称或ID", "添加好友").OnSubmit += OnFriendAddSubmit;
	}

    private bool OnFriendAddSubmit(string inputText, out string tips)
    {
		tips = string.Empty;
		int friendId = 0;
		string friendName = string.Empty;
		if (!int.TryParse(inputText, out friendId))
			friendName = inputText;
		if(friendId == User.Instance.CurrentCharacter.Id || friendName == User.Instance.CurrentCharacter.Name)
        {
			tips = "开玩笑嘛？不能添加自己哦";
			return false;
        }
		FriendService.Instance.SendFriendAddRequest(friendId, friendName);
		return true;
    }
	public void OnClickFriendChat()
    {
		MessageBox.Show("功能未完成");
    }
	public void OnClickFriendRemove()
    {
        if (selectedItem == null)
        {
			MessageBox.Show("请选择好友");
			return;
        }
		MessageBox.Show(String.Format("确定删除好友【{0}】嘛？", selectedItem.nickname.text), "删除好友", MessageBoxType.Confirm, "删除", "取消")
			.OnYes = () => { FriendService.Instance.SendFriendRemoveRequest(this.selectedItem.Info.Id, this.selectedItem.Info.friendInfo.Id); };
    }

    public void RefreshUI()
    {
		ClearFriendList();
		InitFriendItems();
    }

	public void ClearFriendList()
    {
		this.listMain.RemoveAll();
	}
	public void InitFriendItems()
    {
		foreach (var item in FriendManager.Instance.allFriends)
		{
			GameObject go = Instantiate(itemPrefab, itemRoot);
			UIFriendItem friendItem = go.GetComponent<UIFriendItem>();
			friendItem.SetFriendInfo(item);
			this.listMain.AddItem(friendItem);
		}
	}
	private void OnDestroy()
    {
		FriendService.Instance.OnFriendUpdate -= RefreshUI;

	}
}
