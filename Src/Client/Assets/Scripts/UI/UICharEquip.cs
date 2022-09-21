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
    public Text nameText;
    public Text levelText;

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
        UpdateIntroduce();
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
            for(int i = 0; i < tr.childCount; i++)
            {
                UIEquipItem uIEquipItem = tr.GetChild(i).GetComponent<UIEquipItem>();
                if (uIEquipItem != null) Destroy(uIEquipItem.gameObject);
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
    private void UpdateIntroduce()
    {
        this.nameText.text = User.Instance.CurrentCharacter.Name;
        this.levelText.text = "Lv. " + User.Instance.CurrentCharacter.Level.ToString();
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
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
