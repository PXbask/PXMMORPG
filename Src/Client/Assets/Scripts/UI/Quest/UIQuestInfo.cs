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

    public Transform[] slots;
    public GameObject iconItemPrefab;

    public Text rewardMoney;
    public Text rewardExp;
    void Start() { }
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
    public void InitIconItems(Quest quest)
    {
        if (quest.Define.RewardItem1 > 0)
        {
            slots[0].gameObject.SetActive(true);
            UIIconItem iconItem = slots[0].GetComponentInChildren<UIIconItem>();

            if (iconItem == null)
            {
                GameObject go = Instantiate(iconItemPrefab,slots[0]);
                iconItem = go.GetComponent<UIIconItem>();
            }
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

            if (iconItem == null)
            {
                GameObject go = Instantiate(iconItemPrefab, slots[1]);
                iconItem = go.GetComponent<UIIconItem>();
            }
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

            if (iconItem == null)
            {
                GameObject go = Instantiate(iconItemPrefab, slots[2]);
                iconItem = go.GetComponent<UIIconItem>();
            }
            var path = Manager.DataManager.Instance.Items[quest.Define.RewardItem3].Icon;
            iconItem.SetMainIcon(path, quest.Define.RewardItem3Count.ToString());
        }
        else
        {
            slots[2].gameObject.SetActive(false);
        }
    }
}
