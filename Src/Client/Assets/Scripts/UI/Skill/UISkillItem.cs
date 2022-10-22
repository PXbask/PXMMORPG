using Battle;
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

    internal Skill item;

    public override void onSelected(bool selected)
    {
        this.bg.overrideSprite = selected ? selectedbg : normalbg;
    }

    internal void SetItem(Skill value, UISkill owner)
    {
        this.item = value;

        if (title != null) this.title.text = item.Define.Name;
        if (level != null) this.level.text = "Lv. " + item.Info.Level.ToString();
        if (icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
    }
}
