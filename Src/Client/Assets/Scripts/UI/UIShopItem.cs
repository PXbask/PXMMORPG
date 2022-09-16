using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIShopItem : MonoBehaviour, ISelectHandler {

    public Text nameText;
    public Text countText;
    public Text priceText;
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
    public int ShopItemID { get; set; }
    public UIShop uiShop;
    private Common.Data.ItemDefine itemDefine;
    private Common.Data.ShopItemDefine shopItemDefine;

    public void SetInfo(int id, Common.Data.ShopItemDefine define,UIShop owner)
    {
        this.uiShop = owner;
        this.ShopItemID = id;
        this.shopItemDefine = define;

        this.itemDefine=Manager.DataManager.Instance.Items[define.ItemID];

        this.nameText.text=itemDefine.Name;
        this.countText.text = shopItemDefine.Count.ToString();
        this.priceText.text = shopItemDefine.Price.ToString();
        this.itemImage.overrideSprite = Resloader.Load<Sprite>(itemDefine.Icon);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Selected = true;
        this.uiShop.SelectShopItem(this);
    }
}
