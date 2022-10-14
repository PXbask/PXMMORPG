using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestInfo : MonoBehaviour {

    public Text title;
    public Text[] targets;
    public Text description;

    public List<Transform> slots;

    public Text rewardMoney;
    public Text rewardExp;

    public Button navButton;
    private int npc = 0;
    void Start() {
        slots.Add(GameObject.Find("UIEquipSlot").transform);
        slots.Add(GameObject.Find("UIEquipSlot (1)").transform);
        slots.Add(GameObject.Find("UIEquipSlot (2)").transform);
    }
    public void SetQuestInfo(Quest quest)
    {
        this.title.text = string.Format("[{0}] {1}", quest.Define.Type, quest.Define.Name);
        if (quest.Info == null)
        {
            this.description.text = quest.Define.Dialog;
        }
        else
        {
            if (quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
            {
                this.description.text = quest.Define.DialogFinish;
            }
        }
        this.rewardMoney.text = quest.Define.RewardGold.ToString();
        this.rewardExp.text = quest.Define.RewardExp.ToString();

        if(quest.Info == null)
        {
            this.npc = quest.Define.AcceptNPC;
        }
        else if(quest.Info.Status==SkillBridge.Message.QuestStatus.Complated)
        {
            this.npc = quest.Define.SubmitNPC;
        }
        this.navButton.gameObject.SetActive(npc > 0);

        this.InitIconItems(quest);
        foreach(var fitter in GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }
    }
    void Update() { }
    public void OnClickAbandon()
    {

    }
    public void OnClickNav()
    {
        Vector3 pos = Manager.NPCManager.Instance.GetNpcPosition(npc);
        Models.User.Instance.CurrentCharacterObject.StartNav(pos);
        Manager.UIManager.Instance.Close<UIQuestSystem>();
    }
    public void InitIconItems(Quest quest)
    {
        if (quest.Define.RewardItem1 > 0)
        {
            slots[0].gameObject.SetActive(true);
            UIIconItem iconItem = slots[0].GetComponentInChildren<UIIconItem>();

            var path = Manager.DataManager.Instance.Items[quest.Define.RewardItem1].Icon;
            iconItem.SetMainIcon(path, quest.Define.RewardItem1Count.ToString());
        }
        else
        {
            slots[0].gameObject.SetActive(false);
        }

        if (quest.Define.RewardItem2 > 0)
        {
            slots[1].gameObject.SetActive(true);
            UIIconItem iconItem = slots[1].GetComponentInChildren<UIIconItem>();

            var path = Manager.DataManager.Instance.Items[quest.Define.RewardItem2].Icon;
            iconItem.SetMainIcon(path, quest.Define.RewardItem2Count.ToString());
        }
        else
        {
            slots[1].gameObject.SetActive(false);
        }

        if (quest.Define.RewardItem3 > 0)
        {
            slots[2].gameObject.SetActive(true);
            UIIconItem iconItem = slots[2].GetComponentInChildren<UIIconItem>();

            var path = Manager.DataManager.Instance.Items[quest.Define.RewardItem3].Icon;
            iconItem.SetMainIcon(path, quest.Define.RewardItem3Count.ToString());
        }
        else
        {
            slots[2].gameObject.SetActive(false);
        }
    }
}
