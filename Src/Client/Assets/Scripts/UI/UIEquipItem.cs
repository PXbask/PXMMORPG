using Manager;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEquipItem : MonoBehaviour, UnityEngine.EventSystems.IPointerClickHandler
{
    public Text nameText;
    public Text levelText;
    public Text limitClass;
    public Text limitCategory;
    public Image itemImage;
    public Image BgdImage;
    public Sprite normalSprite;
    public Sprite selectedSprite;

    private bool selected;
    public bool Selected
    {
        get
        {
            return selected;
        }
        set
        {
            BgdImage.overrideSprite = value ? selectedSprite : normalSprite;
            selected = value;
        }
    }
    public int Index { get; set; }
    private UICharEquip owner;
    private Item item;
    private bool isEquiped = false;
    internal void SetEquipItem(int idx, Item item, UICharEquip owner, bool equiped)
    {
        this.owner = owner;
        this.Index = idx;
        this.item = item;
        this.isEquiped = equiped;
        if (this.nameText != null) this.nameText.text = this.item.Define.Name;
        if (this.levelText != null) this.levelText.text = item.Define.Level.ToString();
        if (this.limitClass != null) this.limitClass.text = item.Define.LimitClass.ToString();
        if (this.limitCategory != null) this.limitCategory.text = item.Define.Category;
        if (this.itemImage != null) this.itemImage.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.isEquiped)
        {
            UnEquip();
        }
        else
        {
            if (this.selected)
            {
                DoEquip();
                this.Selected = false;
            }
            else
            {
                this.Selected = true;
            }
        }
    }

    private void DoEquip()
    {
        var msg = MessageBox.Show(string.Format("要装备{0}吗?", this.item.Define.Name), "确认", MessageBoxType.Confirm);
        msg.OnYes += () =>
        {
            var oldEquip = EquipManager.Instance.GetEquip(item.EquipDefine.Slot);
            if(oldEquip != null)
            {
                var newmsg=MessageBox.Show(string.Format("要替换装备{0}吗?", oldEquip.Define.Name), "确认", MessageBoxType.Confirm);
                newmsg.OnYes += () => { this.owner.DoEquip(this.item); };
            }
            else
            {
                this.owner.DoEquip(this.item);
            }
        };
    }

    private void UnEquip()
    {
        var msg = MessageBox.Show(string.Format("确定要脱掉{0}吗?", item.Define.Name), "确认", MessageBoxType.Confirm);
        msg.OnYes += () => { this.owner.UnEquip(this.item); };
    }
}
