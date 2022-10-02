using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using GameServer.Services;
using Network;
using Common;

namespace GameServer.Managers
{
    internal class FriendManager : IPostResponser
    {
        private Character Owner;
        List<NFriendInfo> friends=new List<NFriendInfo>();
        bool friendChanged = false;

        public FriendManager(Character character)
        {
            this.Owner = character;
            this.InitFriends();
        }
        internal void GetFriendInfos(List<NFriendInfo> friends)
        {
            foreach(var f in this.friends)
            {
                friends.Add(f);
            }
        }
        public void InitFriends()
        {
            this.friends.Clear();
            foreach(var friend in this.Owner.Data.Friends)
            {
                this.friends.Add(GetFriendInfo(friend));
            }
        }

        public void AddFriend(Character friend)
        {
            TCharacterFriend tf = new TCharacterFriend()
            {
                FriendID = friend.Id,
                FriendName = friend.Data.Name,
                Class = friend.Data.Class,
                Level = friend.Data.Level,
            };
            this.Owner.Data.Friends.Add(tf);
            friendChanged = true;
        }
        public bool RemoveFriendByFriendID(int friendId)
        {
            var removeItem = this.Owner.Data.Friends.FirstOrDefault(x => x.FriendID == friendId);
            if(removeItem != null)
            {
                DBService.Instance.Entities.CharacterFriends.Remove(removeItem);
            }
            friendChanged = true;
            return true;
        }
        public bool RemoveFriendByID(int id)
        {
            var removeItem = this.Owner.Data.Friends.FirstOrDefault(x => x.Id == id);
            if (removeItem != null)
            {
                DBService.Instance.Entities.CharacterFriends.Remove(removeItem);
            }
            friendChanged = true;
            return true;
        }
        public NFriendInfo GetFriendInfo(TCharacterFriend friend)
        {
            NFriendInfo friendInfo = new NFriendInfo();
            var character = CharacterManager.Instance.GetCharacter(friend.FriendID);
            friendInfo.friendInfo = new NCharacterInfo();
            friendInfo.Id = friend.Id;
            if(character == null)
            {
                friendInfo.friendInfo.Id = friend.FriendID;
                friendInfo.friendInfo.Name = friend.FriendName;
                friendInfo.friendInfo.Class = (CharacterClass)friend.Class;
                friendInfo.friendInfo.Level = friend.Level;
                friendInfo.Status = 0;
            }
            else
            {
                friendInfo.friendInfo = character.GetBasicInfo();
                friendInfo.friendInfo.Name = character.Info.Name;
                friendInfo.friendInfo.Class = character.Info.Class;
                if(friend.Level != character.Info.Level)
                {
                    friend.Level = character.Info.Level;
                }
                friendInfo.friendInfo.Level = character.Info.Level;
                character.FriendManager.UpdateFriendInfo(this.Owner.Info, 1);
                friendInfo.Status = 1;
            }
            Log.InfoFormat("{0}:{1} GetFriendInfo {2}:{3} Status:{4}",
                this.Owner.Id, this.Owner.Info.Name, friendInfo.friendInfo.Id, friendInfo.friendInfo.Name, friendInfo.Status);
            return friendInfo;
        }
        internal NFriendInfo GetFriendInfo(int toId)
        {
            foreach(var f in this.friends)
            {
                if(f.friendInfo.Id == toId)
                {
                    return f;
                }
            }
            return null;
        }
        public void UpdateFriendInfo(NCharacterInfo info, int status)
        {
           foreach(var f in this.friends)
            {
                if(f.friendInfo.Id == info.Id)
                {
                    f.Status = status;
                    break;
                }
            }
           this.friendChanged = true;
        }
        public void OfflineNotify()
        {
            foreach(var friendInfo in this.friends)
            {
                var friend = CharacterManager.Instance.GetCharacter(friendInfo.friendInfo.Id);
                if(friend != null)
                {
                    friend.FriendManager.UpdateFriendInfo(this.Owner.Info, 0);
                }
            }
        }
        public void PostProcess(NetMessageResponse response)
        {
            if (friendChanged)
            {
                Log.InfoFormat("PostProcess > FriendManager: characterId:{0}:{1}", this.Owner.Id, this.Owner.Info.Name);
                this.InitFriends();
                if(response.friendList == null)
                {
                    response.friendList=new FriendListResponse();
                    response.friendList.Friends.AddRange(this.friends);
                }
                friendChanged = false;
            }
        }
    }
}
