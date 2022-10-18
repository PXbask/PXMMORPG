using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using System;
using Services;
using Common.Data;

namespace Manager
{
    public class EquipManager : Singleton<EquipManager>
    {

        public delegate void OnEquipChangeHandler();
        public event OnEquipChangeHandler OnEquipChange;
        public Item[] Equips = new Item[(int)EquipSlot.SlotMax];
        byte[] Data;

        unsafe public void Init(byte[] data)
        {
            this.Data = data;
            this.ParseEquipData(data);
        }
        public bool Contain(int key)
        {
            foreach (Item item in Equips)
            {
                if(item!=null && item.ID == key)
                    return true;
            }
            return false;
        }
        public Item GetEquip(EquipSlot slot)
        {
            return Equips[(int)slot];
        }
        unsafe private void ParseEquipData(byte[] data)
        {
            fixed(byte* dataPtr = data)
            {
                for(int i = 0; i < (int)EquipSlot.SlotMax; i++)
                {
                    int* itemID = (int*)(dataPtr + i * sizeof(int));
                    if (*itemID > 0)
                        this.Equips[i] = ItemManager.Instance.Items[*itemID];
                    else
                        this.Equips[i] = null;
                }
            }
        }

        public List<EquipDefine> GetEquipedDefines()
        {
            List<EquipDefine> res = new List<EquipDefine>();
            for (int i = 0; i < (int)EquipSlot.SlotMax; i++)
            {
                if(Equips[i]!=null)
                    res.Add(Equips[i].EquipDefine);
            }
            return res;
        }

        unsafe private byte[] GetEquipData()
        {
            fixed (byte* dataPtr = this.Data)
            {
                for (int i = 0; i < (int)EquipSlot.SlotMax; i++)
                {
                    int* itemID = (int*)(dataPtr + i * sizeof(int));
                    if (this.Equips[i] != null)
                        *itemID = (this.Equips[i].ID);
                    else
                        *itemID = 0;
                }
            }
            return this.Data;
        }
        public void EquipItem(Item equip)
        {
            ItemService.Instance.SendItemEquip(equip, true);
        }
        public void UnEquipItem(Item equip)
        {
            ItemService.Instance.SendItemEquip(equip, false);
        }
        public void OnEquipItem(Item equip)
        {
            if (this.Equips[(int)equip.EquipDefine.Slot] != null && this.Equips[(int)equip.EquipDefine.Slot].ID == equip.ID)
                return;
            this.Equips[(int)equip.EquipDefine.Slot] = ItemManager.Instance.Items[equip.ID];
            if (OnEquipChange != null)
                OnEquipChange();
        }
        public void OnUnEquipItem(EquipSlot slot)
        {
            if (this.Equips[(int)slot] != null)
            {
                this.Equips[(int)slot] = null;
                if (OnEquipChange != null)
                    OnEquipChange();
            }
        }
    }
}


