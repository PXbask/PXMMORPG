using Common;
using Common.Data;
using GameServer.Core;
using GameServer.Managers;
using GameServer.Models;
using Manager;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
    /// <summary>
    /// Character
    /// 玩家角色类
    /// </summary>
    internal class Character : CharacterBase, IPostResponser
    {
        public Team team;
        public double TeamUpdateTS;

        public Guild Guild;
        public double GuildUpdateTS;

        public Chat Chat;

        public TCharacter Data;
        public ItemManager ItemManager;
        public StatusManager StatusManager;
        public QuestManager QuestManager;
        public FriendManager FriendManager;
        public Character(CharacterType type,TCharacter cha):base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ),new Core.Vector3Int(100,0,0))
        {
            this.Data = cha;
            this.Id = cha.ID;
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Id = cha.ID;
            this.Info.EntityId = this.entityId;
            this.Info.Name = cha.Name;
            this.Info.Level = 10;
            this.Info.ConfigId = cha.TID;
            this.Info.Gold = cha.Gold;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
				this.Info.Ride = 0;
            this.Info.Entity = this.EntityData;
            this.Define = DataManager.Instance.Characters[this.Info.ConfigId];
            this.ItemManager=new ItemManager(this);
            this.ItemManager.GetItemInfos(this.Info.Items);

            this.Info.Bag=new NBagInfo();
            this.Info.Bag.Unlocked = Data.Bag.Unlocked;
            this.Info.Bag.Items=Data.Bag.Items;
            this.Info.Equips=Data.Equips;

            this.StatusManager = new StatusManager(this);

            this.QuestManager = new QuestManager(this);
            this.QuestManager.GetQuestInfos(this.Info.Quests);

            this.FriendManager = new FriendManager(this);
            this.FriendManager.GetFriendInfos(this.Info.Friends);

            this.Guild = GuildManager.Instance.GetGuild(this.Data.GuildID);

            this.Chat = new Chat(this);
        }
        public long Gold
        {
            get { return this.Data.Gold; }
            set
            {
                if (this.Data.Gold == value)
                    return;
                this.StatusManager.AddGoldChange((int)(value - this.Data.Gold));
                this.Data.Gold = value; 
            }
        }
        public NCharacterInfo GetBasicInfo()
        {
            var info = this.Info;
            return new NCharacterInfo()
            {
                Id = Id,
                Name = info.Name,
                Level = info.Level,
                Class = info.Class,
            };
        }
        public void PostProcess(NetMessageResponse response)
        {
            this.FriendManager.PostProcess(response);

            if (team != null)
            {
                Log.InfoFormat("PostProcess > Team character:{0}:{1} {2} > {3}", this.Id, this.Info.Name, this.TeamUpdateTS, this.team.timeStamp);
                if (this.TeamUpdateTS < this.team.timeStamp)
                {
                    this.TeamUpdateTS = this.team.timeStamp;
                    this.team.PostProcess(response);
                }
            }
            if (Guild != null)
            {
                Log.InfoFormat("PostProcess > Guild character:{0}:{1} {2} > {3}", this.Id, this.Info.Name, this.GuildUpdateTS, this.Guild.timestamp);
                if (this.Info.Guild == null)
                {
                    this.Info.Guild = this.Guild.GuildInfo(this);
                    if (response.mapCharacterEnter != null)
                    {
                        GuildUpdateTS = Guild.timestamp;
                    }
                    if(this.TeamUpdateTS < this.Guild.timestamp && response.mapCharacterEnter == null)
                    {
                        GuildUpdateTS = Guild.timestamp;
                        this.Guild.PostProcess(this,response);
                    }
                }

                if (this.GuildUpdateTS < this.Guild.timestamp)
                {
                    this.GuildUpdateTS = this.Guild.timestamp;
                    this.Guild.PostProcess(this,response);
                }
            }
            if (this.StatusManager.HasStatus)
                this.StatusManager.PostProcess(response);
            this.Chat.PostProcess(response);
        }
		public int Ride
        {
            get { return this.Info.Ride; }
            set
            {
                if (this.Info.Ride == value)
                    return;
                this.Info.Ride = value;
            }
        }
        /// <summary>
        /// 角色离开时调用
        /// </summary>
        public void Clear()
        {
            this.FriendManager.OfflineNotify();
        }
    }
}
