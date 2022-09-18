using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using System;
using Common.Data;

namespace Manager
{
    public class BagManager : Singleton<BagManager>
    {

        public int Unlocked;
        public BagItem[] Items;
        private NBagInfo Info;
        public UIBag uIBag;

        public void ChangeMoneyText()
        {
            if(uIBag != null)
                uIBag.SetMoneyText();
        }
        unsafe public void Init(NBagInfo info)
        {
            User.Instance.OnGoldChange += this.ChangeMoneyText;
            this.Info = info;
            this.Unlocked = info.Unlocked;
            Items = new BagItem[this.Unlocked];
            if (info.Items != null && info.Items.Length >= this.Unlocked)
            {
                Analyze(info.Items);
            }
            else
            {
                Info.Items = new byte[sizeof(BagItem) * this.Unlocked];
                Reset();
            }
        }

        public void Reset()
        {
            int i = 0;
            foreach (var kv in Manager.ItemManager.Instance.Items)
            {
                if (kv.Value.Count <= kv.Value.Define.StackLimit)
                {
                    this.Items[i].ItemID = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)kv.Value.Count;
                }
                else
                {
                    int count = kv.Value.Count;
                    while (count > kv.Value.Define.StackLimit)
                    {
                        this.Items[i].ItemID = (ushort)kv.Key;
                        this.Items[i].Count = (ushort)kv.Value.Define.StackLimit;
                        count -= kv.Value.Define.StackLimit;
                        i++;
                    }
                    this.Items[i].ItemID = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)count;
                }
                i++;
            }
        }
        internal void AddItem(int id, int count)
        {
            ushort addCount = (ushort)count;
            ItemDefine define = DataManager.Instance.Items[id];
            for(int i = 0; i < Items.Length; i++)
            {
                if(this.Items[i].ItemID == id)
                {
                    ushort canAdd=(ushort)(DataManager.Instance.Items[id].StackLimit-Items[i].Count);
                    if(canAdd >= addCount)
                    {
                        this.Items[i].Count += addCount;
                        addCount = 0;
                        break;
                    }
                    else
                    {
                        this.Items[i].Count += canAdd;
                        addCount -= canAdd;
                    }
                }
            }
            if(addCount > 0)
            {
                for(int i = 0; i < Items.Length; i++)
                {
                    if (this.Items[i].ItemID == 0)
                    {
                        if (addCount <= define.StackLimit)
                        {
                            this.Items[i].ItemID = (ushort)id;
                            this.Items[i].Count = (ushort)addCount;
                        }
                        else
                        {
                            int remainCount = addCount;
                            while (remainCount > define.StackLimit)
                            {
                                this.Items[i].ItemID=(ushort)id;
                                this.Items[i].Count=(ushort)define.StackLimit;
                                remainCount -= define.StackLimit;
                                i++;
                            }
                            this.Items[i].ItemID = (ushort)id;
                            this.Items[i].Count = (ushort)remainCount;
                        }
                        break;
                    }
                }
            }
        }
        internal void RemoveItem(int id, int count) { }

        unsafe private void Analyze(byte[] data)
        {
            fixed (byte* pt = data)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    Items[i] = *item;
                }
            }
        }
        unsafe public NBagInfo GetBagInfo()
        {
            fixed (byte* pt = this.Info.Items)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    *item = Items[i];
                }
            }
            return this.Info;
        }
    }
}

