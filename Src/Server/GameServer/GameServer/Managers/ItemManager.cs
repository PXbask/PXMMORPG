using GameServer.Entities;
using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Common;
using GameServer.Services;
using SkillBridge.Message;

namespace GameServer.Managers
{
    internal class ItemManager
    {
        Character Owner;
        public Dictionary<int,Item> Items=new Dictionary<int,Item>();
        public ItemManager(Character character)
        {
            this.Owner = character;
            foreach(var t in Owner.Data.Items)
            {
                this.Items.Add(t.ItemID, new Item(t));
            }
        }
        public bool UseItem(int itemId,int count = 1)
        {
            Log.InfoFormat("[{0}] UseItem [{1}:{2}]",this.Owner.Data.ID,itemId,count);
            Item item = null;
            if(this.Items.TryGetValue(itemId, out item))
            {
                if (item.Count < count)
                    return false;
                //TODO:增加使用逻辑
                item.Remove(count);
                return true;
            }
            return false;
        }
        public bool HasItem(int itemID)
        {
            Item item = null;
            if (this.Items.TryGetValue(itemID, out item))
            {
                return item.Count > 0;
            }
            return false;
        }
        public Item GetItem(int itemID)
        {
            Item item = null;
            this.Items.TryGetValue(itemID, out item);
            Log.InfoFormat("[{0}] GetItem [{1}:{2}]", this.Owner.Data.ID, itemID, item.ToString());
            return item;
        }
        public bool AddItem(int itemID,int count)
        {
            try
            {
                Item item = null;
                if (this.Items.TryGetValue((int)itemID, out item))
                {
                    item.Add(count);
                }
                else
                {
                    TCharacterItem dbItem = new TCharacterItem();
                    dbItem.CharacterID = Owner.Data.ID;
                    dbItem.Owner = Owner.Data;
                    dbItem.ItemID = itemID;
                    dbItem.ItemCount = count;
                    Owner.Data.Items.Add(dbItem);

                    item = new Item(dbItem);
                    this.Items.Add((int)itemID, item);
                }
                Log.InfoFormat("[{0}]AddItem[{1}] addCount:{2}", this.Owner.Data.ID, itemID, count);
                //DBService.Instance.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveItem(int itemID,int count)
        {
            if (!this.Items.ContainsKey(itemID))
            {
                return false;
            }
            Item item = this.Items[itemID];
            if (item.Count < count)
            {
                return false;
            }
            item.Remove(count);
            Log.InfoFormat("[{0}]RemoveItem[{1}] removeCount:{2}", this.Owner.Data.ID, itemID, count);
            //DBService.Instance.Save();
            return true;
        }
        public void GetItemInfos(List<NItemInfo> list)
        {
            foreach (var item in this.Items)
            {
                list.Add(new NItemInfo() { Id=item.Value.ItemID, Count = item.Value.Count });
            }
        }
    }
}
