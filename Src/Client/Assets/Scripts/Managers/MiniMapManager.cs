using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
	public class MiniMapManager : Singleton<MiniMapManager>
	{
        public Transform PlayerTransform
		{
			get
			{
				if (User.Instance.CurrentCharacterObject == null)
					return null;
				else
					return User.Instance.CurrentCharacterObject.transform;
			}
		}

        void Start()
		{

		}

		void Update()
		{

		}
		public Sprite LoadCurrentMiniMap()
		{
			string path = string.Format("UI/Minimap/{0}", User.Instance.CurrentMapData.Asset);
			return Resloader.Load<Sprite>(path);
		}
	}
}

