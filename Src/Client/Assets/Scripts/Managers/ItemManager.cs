using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using SkillBridge.Message;
using Common.Data;

namespace Manager
{
    public class ItemManager : Singleton<ItemManager>
    {
        public Dictionary<int,Item> Items=new Dictionary<int,Item>();
        public void Init(List<NItemInfo> nItems)
        {
            this.Items.Clear();
            foreach (var nitem in nItems)
            {
                Item item = new Item(nitem);
                Items.Add(item.ID,item);
                Debug.LogFormat("ItemManager:Init[{0}]", item.ToString());
            }
        }
        public ItemDefine GetItem(int itemID)
        {
            return null;
        }
        public bool UseItem(int itemID,int count = 1)
        {
            return false;
        }
        public bool UseItem(ItemDefine item)
        {
            return false;
        }
    }
}

