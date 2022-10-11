using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manager;
using UnityEngine.Events;

namespace Models
{
    public class User : Singleton<User>
    {
        public UnityAction OnGoldChange;
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

        public PlayerInputController CurrentCharacterObject { get; set; }
        public SkillBridge.Message.NTeamInfo TeamInfo { get; set; }
        public void AddGold(int value)
        {
            this.CurrentCharacter.Gold += value;
            OnGoldChange();
        }
        public int CurrentRide = 0;
        internal void Ride(int id)
        {
            if (CurrentRide != id)
            {
                CurrentRide = id;
                CurrentCharacterObject.SendEntityEvent(SkillBridge.Message.EntityEvent.Ride, CurrentRide);
            }
            else
            {
                CurrentRide = 0;
                CurrentCharacterObject.SendEntityEvent(SkillBridge.Message.EntityEvent.Ride, 0);
            }
        }
    }
}
