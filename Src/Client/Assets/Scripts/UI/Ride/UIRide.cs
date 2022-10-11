using Manager;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRide : UIWindow {

    public Text description;
    public GameObject itemPrefab;
    public ListView listMain;
    private UIRideItem selectedItem;

    protected override void OnStart()
    {
        RefreshUI();
        this.listMain.onItemSelected += this.OnItemSelected;
    }

    private void OnItemSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIRideItem;
        this.description.text = this.selectedItem.item.Define.Description;
    }

    private void RefreshUI()
    {
        ClearItems();
        InitItems();
    }

    private void InitItems()
    {
        foreach (var kv in ItemManager.Instance.Items)
        {
            if (kv.Value.Define.Type == SkillBridge.Message.ItemType.Ride && 
                (kv.Value.Define.LimitClass==SkillBridge.Message.CharacterClass.None) ||
                (kv.Value.Define.LimitClass==User.Instance.CurrentCharacter.Class))
            {
                if (EquipManager.Instance.Contain(kv.Key))
                    continue;
                GameObject go = Instantiate(itemPrefab, listMain.transform);
                UIRideItem ui = go.GetComponent<UIRideItem>();
                ui.SetEquipItem(kv.Value, this);
                this.listMain.AddItem(ui);
            }
        }
    }

    private void ClearItems()
    {
        this.listMain.RemoveAll();
    }
    public void OnClickRide()
    {
        if(this.selectedItem == null)
        {
            MessageBox.Show("请选择要召唤的坐骑", "提示");
            return;
        }
        User.Instance.Ride(this.selectedItem.item.ID);
    }
}
