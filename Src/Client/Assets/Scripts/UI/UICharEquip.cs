using Manager;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharEquip : UIWindow {
    public Text title;
    public Text money;

    public GameObject itemPrefab;
    public GameObject itemEquipedPrefab;

    public Transform itemListRoot;
    public List<Transform> slots;

    protected override void OnStart()
    {
        RefreshUI();
        EquipManager.Instance.OnEquipChange += RefreshUI;
    }
    private void OnDestroy()
    {
        EquipManager.Instance.OnEquipChange -= RefreshUI;
    }
    private void RefreshUI()
    {
        ClearAllEquipList();
        InitAllEquipItems();
        ClearEquipedList();
        InitEquipedItems();
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }
    private void ClearAllEquipList()
    {
        foreach (var com in itemListRoot.GetComponentsInChildren<UIEquipItem>())
            Destroy(com.gameObject);
    }
    private void InitAllEquipItems()
    {
        foreach(var kv in ItemManager.Instance.Items)
        {
            if(kv.Value.Define.Type == SkillBridge.Message.ItemType.Equip)
            {
                if (EquipManager.Instance.Contain(kv.Key))
                    continue;
                GameObject go = Instantiate(itemPrefab, itemListRoot);
                UIEquipItem ui=go.GetComponent<UIEquipItem>();
                ui.SetEquipItem(kv.Key, kv.Value, this, false);
            }
        }
    }
    private void ClearEquipedList()
    {
        foreach(var tr in slots)
        {
            if (tr.childCount > 0)
            {
                Destroy(tr.GetChild(0).gameObject);
            }
        }
    }
    private void InitEquipedItems()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            Item item = Manager.EquipManager.Instance.Equips[i];
            if (item != null)
            {
                GameObject go = Instantiate(itemEquipedPrefab, slots[i]);
                UIEquipItem uI = go.GetComponent<UIEquipItem>();
                uI.SetEquipItem(item.ID,item, this, true);
            }
        }
    }
    public void DoEquip(Item item)
    {
        EquipManager.Instance.EquipItem(item);
    }
    public void UnEquip(Item item)
    {
        EquipManager.Instance.UnEquipItem(item);
    }
}
