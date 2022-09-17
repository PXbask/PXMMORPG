using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using SkillBridge.Message;
using Common.Data;
using Services;
using System;

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
            StatusService.Instance.RegisterStatusNotify(StatusType.Item, OnItemNotify);
        }

        private bool OnItemNotify(NStatus status)
        {
            if (status.Action == StatusAction.Add)
                this.AddItem(status.Id, status.Value);
            if(status.Action == StatusAction.Delete)
                this.RemoveItem(status.Id, status.Value);
            return true;
        }
        private void AddItem(int id, int count)
        {
            Item item = null;
            if(this.Items.TryGetValue(id, out item))
            {
                item.Count += count;
            }
            else
            {
                item=new Item(id, count);
                this.Items.Add(id, item);
            }
            BagManager.Instance.AddItem(id, count);
        }
        private void RemoveItem(int id, int count)
        {
            if (!this.Items.ContainsKey(id))
            {
                return;
            }
            Item item = this.Items[id];
            if (item.Count < count)
                return;
            item.Count -= count;
            BagManager.Instance.RemoveItem(id, count);
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

