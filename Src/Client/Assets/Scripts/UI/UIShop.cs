using Common.Data;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIWindow {

	public Text title;
	public Text money;
	public GameObject uishopItem;
	private ShopDefine shopDefine;
	public Transform itemRoot;

	protected override void OnStart()
    {
		StartCoroutine(InitItems());
    }

    private IEnumerator InitItems()
    {
        foreach(var kv in Manager.DataManager.Instance.ShopItems[shopDefine.ID])
        {
            if (kv.Value.Status > 0)
            {
                GameObject go=Instantiate(uishopItem,itemRoot);
                UIShopItem uIShopItem = go.GetComponent<UIShopItem>();
                uIShopItem.SetInfo(kv.Key, kv.Value, this);
            }
        }
        yield return null;
    }
    public void SetShop(ShopDefine shop)
    {
        this.shopDefine = shop;
        this.title.text = shop.Name;
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }
    private UIShopItem selectedItem;
    public void SelectShopItem(UIShopItem item)
    {
        if(selectedItem != null)
        {
            selectedItem.Selected = false;
        }
        selectedItem = item;
    }
    public void OnClickBuy()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("请选择要购买的物品", "提示");
            return;
        }
        else
        {
            if(Manager.ShopManager.Instance.BuyItem(this.shopDefine.ID, selectedItem.ShopItemID))
            {
                MessageBox.Show("Purchase Successfully！", "提示");
            }        
        }
    }
}
