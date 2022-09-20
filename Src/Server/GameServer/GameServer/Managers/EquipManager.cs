using Common;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using GameServer.Entities;
using GameServer.Services;

namespace GameServer.Managers
{
    class EquipManager:Singleton<EquipManager>
    {
        public Result EquipItem(NetConnection<NetSession> sender,int slot,int itemID,bool isEquip)
        {
            Character character = sender.Session.Character;
            if(character.ItemManager.Items.ContainsKey(itemID))
                return Result.Failed;
            UpdateEquip(character.Data.Equips, slot, itemID, isEquip);
            DBService.Instance.Save();
            return Result.Success;
        }

        unsafe private void UpdateEquip(byte[] equipData, int slot, int itemID, bool isEquip)
        {
            fixed (byte* equipDataPtr = equipData)
            {
                int* slotID = (int*)(equipDataPtr + slot * sizeof(int));
                if (isEquip)
                    *slotID = itemID;
                else
                    *slotID = 0;
            }
        }
    }
}
