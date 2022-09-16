﻿using Common;
using GameServer.Entities;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    public class BagService
    {
        public BagService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<BagSaveRequest>(this.OnBagSave);
        }
        public void Init() { }

        private void OnBagSave(NetConnection<NetSession> sender, BagSaveRequest request)
        {
            Character character=sender.Session.Character;
            if (request.BagInfo != null)
            {
                Log.InfoFormat("BagSaveRequest: character:{0} Unlocked:{1}", character.Id, request.BagInfo.Unlocked);
                character.Data.Bag.Items = request.BagInfo.Items;
                DBService.Instance.Save();
            }
        }
    }
}
