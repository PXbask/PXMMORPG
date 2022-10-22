using Common.Data;
using Manager;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkill : UIWindow {

	public Text descr;
	public GameObject itemPrefab;
	public ListView listMain;
	public UISkillItem selectedItem;

    protected override void OnStart()
    {
        RefreshUI();
        this.listMain.onItemSelected += this.onItemSelected;
    }
    private void OnDestroy() { }
    private void RefreshUI()
    {
        ClearItems();
        InitItems();
    }
    private void onItemSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UISkillItem;
        this.descr.text = this.selectedItem.item.Define.Description;
    }

    private void InitItems()
    {
        var Skills = User.Instance.CurrentCharacter.SkillMgr.Skills;
        foreach (var skill in Skills)
        {
            if (skill.Define.Type.Equals(SkillType.Skill))
            {
                GameObject go = Instantiate(itemPrefab, listMain.transform);
                UISkillItem ui = go.GetComponent<UISkillItem>();
                ui.SetItem(skill, this);
                listMain.AddItem(ui);
            }
        }
    }

    private void ClearItems()
    {
        listMain.RemoveAll();
    }
}
