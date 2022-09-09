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
			this.UIResources.Add(typeof(UITest), new UIElement() { Resources="UI/UITest",Cache=true});
        }
        ~UIManager() { }
		public T Show<T>()
        {
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
	}
}
