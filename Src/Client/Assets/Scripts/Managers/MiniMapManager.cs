using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : Singleton<MiniMapManager> {

	void Start () {
		
	}
	
	void Update () {
		
	}
	public Sprite LoadCurrentMiniMap()
    {
		string path = string.Format("UI/Minimap/{0}",User.Instance.CurrentMapData.Asset);
		return Resloader.Load<Sprite>(path);
    }
}
