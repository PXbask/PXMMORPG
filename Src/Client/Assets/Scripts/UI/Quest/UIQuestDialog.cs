using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuestDialog : UIWindow
{
    public UIQuestInfo questInfo;
    public Quest quest;
    public GameObject openBtn;
    public GameObject submitBtn;

    protected override void OnStart() { }
    public void SetQuest(Quest quest)
    {
        this.quest = quest;
        this.UpdateQuest();
        if (this.quest.Info == null)
        {
            openBtn.SetActive(true);
            submitBtn.SetActive(false);
        }
        else
        {
            if(this.quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
            {
                openBtn.SetActive(false);
                submitBtn.SetActive(true);
            }
            else
            {
                openBtn.SetActive(false);
                submitBtn.SetActive(false);
            }
        }
    }

    private void UpdateQuest()
    {
        if(this.quest != null)
        {
            if (this.questInfo != null)
            {
                this.questInfo.SetQuestInfo(this.quest);   
            }
        }
    }
}
