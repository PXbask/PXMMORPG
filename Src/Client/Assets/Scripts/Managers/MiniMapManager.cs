using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
	public class MiniMapManager : Singleton<MiniMapManager>
	{
		public UIMiniMap uIMiniMap;
		private Collider boundary;
		public Collider Boundary
        {
			get { return boundary; }
        }

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

		public Sprite LoadCurrentMiniMap()
		{
			string path = string.Format("UI/Minimap/{0}", User.Instance.CurrentMapData.Asset);
			return Resloader.Load<Sprite>(path);
		}
		public void UpdateBoundary(Collider collider)
        {
            if (collider != null)
				this.boundary = collider;
			if(uIMiniMap != null)
				uIMiniMap.UpdateMap();
		}
	}
}

