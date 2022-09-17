﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    class User : Singleton<User>
    {
        SkillBridge.Message.NUserInfo userInfo;


        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }


        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }

        public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }

        public Common.Data.MapDefine CurrentMapData { get; set; }

        public UnityEngine.GameObject CurrentCharacterObject { get; set; }
        public void AddGold(int value)
        {
            this.CurrentCharacter.Gold += value;
        }
    }
}
