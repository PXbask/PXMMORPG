using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using System;

namespace Manager
{
    public class ShopManager : Singleton<ShopManager>
    {
        public void Init()
        {
            NPCManager.Instance.RegisterNpcEvent(NpcFunction.InvokeShop, OnOpenShop);
        }

        private bool OnOpenShop(NpcDefine npc)
        {
            this.ShowShop(npc.Param);
            return true;
        }

        private void ShowShop(int param)
        {
            ShopDefine define = null;
            if(DataManager.Instance.Shops.TryGetValue(param, out define))
            {
                UIShop uiShop = UIManager.Instance.Show<UIShop>();
                if(uiShop != null)
                {
                    uiShop.SetShop(define);
                }
            }
        }
        public bool BuyItem(int shopID,int shopItemID)
        {
            Services.ItemService.Instance.SendBuyItem(shopID, shopItemID);
            return true;
        }
    }
}

