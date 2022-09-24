using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestItem : ListView.ListViewItem {

	public Text title;
	public Image background;
	public Sprite normalBg;
	public Sprite selectedBg;

	public override void onSelected(bool selected)
	{
		this.background.sprite = !selected ? normalBg : selectedBg;
	}
    public Quest quest;
    void Start() { }
    public void SetQuestInfo(Quest quest)
    {
        this.quest = quest;
        if (this.title != null) this.title.text = quest.Define.Name;
    }
}
