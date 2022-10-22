﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Manager
{
	public class UIManager : Singleton<UIManager>
	{
		class UIElement
        {
			public string Resources;
			public bool Cache;
			public GameObject Instance;
        }
		private Dictionary<Type, UIElement> UIResources = new Dictionary<Type, UIElement>();
		public UIManager()
        {
            this.UIResources.Add(typeof(UIBag), new UIElement() { Resources = "UI/UIBag", Cache = false });
            this.UIResources.Add(typeof(UIShop), new UIElement() { Resources = "UI/UIShop", Cache = false });
            this.UIResources.Add(typeof(UICharEquip), new UIElement() { Resources = "UI/UICharEquip", Cache = false });
            this.UIResources.Add(typeof(UIQuestDialog), new UIElement() { Resources = "UI/UIQuestDialog", Cache = false });
            this.UIResources.Add(typeof(UIQuestSystem), new UIElement() { Resources = "UI/UIQuestSystem", Cache = false });
            this.UIResources.Add(typeof(UIFriends), new UIElement() { Resources = "UI/UIFriends", Cache = false });

            this.UIResources.Add(typeof(UIGuild), new UIElement() { Resources = "UI/Guild/UIGuild", Cache = false });
            this.UIResources.Add(typeof(UIGuildList), new UIElement() { Resources = "UI/Guild/UIGuildList", Cache = false });
            this.UIResources.Add(typeof(UIGuildPopNoGuild), new UIElement() { Resources = "UI/Guild/UIGuildPopNoGuild", Cache = false });
            this.UIResources.Add(typeof(UIGuildPopCreate), new UIElement() { Resources = "UI/Guild/UIGuildPopCreate", Cache = false });
            this.UIResources.Add(typeof(UIGuildApplyList), new UIElement() { Resources = "UI/Guild/UIGuildApplyList", Cache = false });

            this.UIResources.Add(typeof(UISetting), new UIElement() { Resources = "UI/UISetting", Cache = false });
            this.UIResources.Add(typeof(UIPopChatMenu), new UIElement() { Resources = "UI/UIPopChatMenu", Cache = false });

            this.UIResources.Add(typeof(UIRide), new UIElement() { Resources = "UI/UIRide", Cache = false });
            this.UIResources.Add(typeof(UISystemConfig), new UIElement() { Resources = "UI/UISystemConfig", Cache = false });
            this.UIResources.Add(typeof(UISkill), new UIElement() { Resources = "UI/UISkill", Cache = false });
        }
        ~UIManager() { }
		public T Show<T>()
        {
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Win_Open);
			Type type = typeof(T);
            if (this.UIResources.ContainsKey(type))
            {
				UIElement info = this.UIResources[type];
				if (info.Instance != null)
                {
					info.Instance.SetActive(true);
                }
                else
                {
                    UnityEngine.Object prefab=Resources.Load(info.Resources);
                    if (prefab == null)
                    {
                        Debug.LogWarningFormat("UI prefab can't find:{0}", type.Name);
                        return default(T);
                    }
                    info.Instance = (GameObject)GameObject.Instantiate(prefab);
                }
                return info.Instance.GetComponent<T>();
            }
            Debug.LogWarningFormat("UI prefab can't find:{0}", type.Name);
            return default(T);
        }
        public void Close(Type type)
        {
            if (this.UIResources.ContainsKey(type))
            {
                UIElement uIElement = this.UIResources[type];
                if (uIElement.Cache)
                {
                    uIElement.Instance.SetActive(false);
                }
                else
                {
                    GameObject.Destroy(uIElement.Instance);
                    uIElement.Instance = null;
                }
            }
        }
        public void Close<T>()
        {
            this.Close(typeof(T));
        }
	}
}
