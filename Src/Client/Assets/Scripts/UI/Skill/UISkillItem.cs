using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillItem : ListView.ListViewItem
{
    public Text title;
    public Text level;
    public Image icon;
    public Image bg;
    public Sprite normalbg;
    public Sprite selectedbg;

    internal SkillDefine item;

    public override void onSelected(bool selected)
    {
        this.bg.overrideSprite = selected ? selectedbg : normalbg;
    }

    internal void SetItem(SkillDefine value, UISkill owner)
    {
        this.item = value;

        if (title != null) this.title.text = item.Name;
        if (level != null) this.level.text = "Lv. " + item.UnlockLevel.ToString();
        if (icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Icon);
    }
}
