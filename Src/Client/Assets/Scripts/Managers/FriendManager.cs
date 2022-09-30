using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;


namespace Manager
{
	public class FriendManager : Singleton<FriendManager>
	{
		public List<NFriendInfo> allFriends;

		public void Init(List<NFriendInfo> friends)
        {
			allFriends = friends;
        }
	}
}

