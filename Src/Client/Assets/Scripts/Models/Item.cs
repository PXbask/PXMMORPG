using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using Common.Data;

namespace Models
{
    public class Item
    {
        public int ID;
        public int Count;
        public ItemDefine Define;
        public EquipDefine EquipDefine;
        public Item(NItemInfo item):this(item.Id, item.Count) { }
        public Item(int itemID,int count)
        {
            ID = itemID;
            Count = count;
            Manager.DataManager.Instance.Items.TryGetValue(itemID,out Define);
            Manager.DataManager.Instance.Equips.TryGetValue(itemID,out EquipDefine);
        }
        public override string ToString()
        {
            return string.Format("ID:{0}, Count:{1}", this.ID, this.Count);
        }
    }
}

