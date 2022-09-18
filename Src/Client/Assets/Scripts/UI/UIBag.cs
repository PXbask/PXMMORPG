using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBag : UIWindow {

    public Text Money;
    public Transform[] Pages;
    public GameObject BagItem;
    private List<Image> Slots;

    protected override void OnStart()
    {
        if (Slots == null)
        {
            Slots = new List<Image>(capacity: 100);
            for (int i = 0; i < Pages.Length; i++)
            {
                for (int j = 0; j < Pages[i].childCount; j++)
                {
                    Slots.Add(Pages[i].GetChild(j).GetChild(0).GetComponent<Image>());
                }
            }
        }
        StartCoroutine(InitBag());
        Manager.BagManager.Instance.uIBag = this;
    }
    private IEnumerator InitBag()
    {
        for(int i = 0; i < Manager.BagManager.Instance.Items.Length; i++)
        {
            var item = Manager.BagManager.Instance.Items[i];
            if (item.ItemID > 0)
            {
                GameObject go = GameObject.Instantiate(BagItem, Slots[i].transform);
                var icon=go.GetComponent<UIIconItem>();
                var def=Manager.DataManager.Instance.Items[item.ItemID];
                icon.SetMainIcon(def.Icon, item.Count.ToString());
            }
        }
        for(int i= Manager.BagManager.Instance.Items.Length; i < Slots.Count; i++)
        {
            Slots[i].color = Color.gray;
        }
        yield return null;
    }
    public void SetMoneyText()
    {
        this.Money.text = Models.User.Instance.CurrentCharacter.Id.ToString();
    }
    public void Refresh()
    {
        //TODO:
        //删除多余的格子
        foreach(var go in this.Slots)
        {
            for(int i = 0; i < go.transform.childCount; i++)
            {
                Destroy(go.transform.GetChild(i).gameObject);
            }
        }
        Manager.BagManager.Instance.Reset();
        this.StartCoroutine(InitBag());
    }
}
