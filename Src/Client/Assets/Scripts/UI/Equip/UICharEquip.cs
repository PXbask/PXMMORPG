using Common.Battle;
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

    public Text hp;
    public Slider hpSlider;
    public Text mp;
    public Slider mpSlider;
    public Text[] attrs;

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
            if(kv.Value.Define.Type == SkillBridge.Message.ItemType.Equip && kv.Value.Define.LimitClass.Equals(User.Instance.CurrentCharacterInfo.Class))
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
        this.nameText.text = User.Instance.CurrentCharacterInfo.Name;
        this.levelText.text = "Lv. " + User.Instance.CurrentCharacterInfo.Level.ToString();
        this.money.text = User.Instance.CurrentCharacterInfo.Gold.ToString();
        this.InitAttributes();
    }

    private void InitAttributes()
    {
        var charattr = User.Instance.CurrentCharacter.Attributes;
        this.hp.text = String.Format("{0}/{1}",charattr.HP,charattr.MaxHP);
        this.mp.text = String.Format("{0}/{1}",charattr.MP,charattr.MaxMP);
        this.hpSlider.maxValue = charattr.MaxHP;
        this.hpSlider.value = charattr.HP;
        this.mpSlider.maxValue = charattr.MaxMP;
        this.mpSlider.value = charattr.MP;

        for(int i = (int)AttributeType.STR; i < (int)AttributeType.MAX; i++)
        {
            var j = (int)AttributeType.STR;
            if (i == (int)AttributeType.CRI)
                this.attrs[i - j].text = String.Format("{0:f2}%", charattr.Final.Data[i] * 100);
            else
                this.attrs[i - j].text = ((int)charattr.Final.Data[i]).ToString();
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
