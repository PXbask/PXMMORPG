using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class ItemService : Singleton<ItemService>, IDisposable
    {
        public ItemService()
        {
            MessageDistributer.Instance.Subscribe<ItemBuyResponse>(this.OnItemBuy);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(this.OnItemBuy);
        }
        internal void SendBuyItem(int shopID, int shopItemID)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemBuy = new ItemBuyRequest();
            message.Request.itemBuy.shopID = shopID;
            message.Request.itemBuy.shopItemID = shopItemID;

            NetClient.Instance.SendMessage(message);
        }
        private void OnItemBuy(object sender, ItemBuyResponse message)
        {
            MessageBox.Show("购买结果:" + message.Result + "\n" + message.Errormsg, "购买完成");
        }
    }
}
