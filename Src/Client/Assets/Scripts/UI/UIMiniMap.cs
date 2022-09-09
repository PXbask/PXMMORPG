using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager;
public class UIMiniMap : MonoBehaviour {
	public Collider boundary;
	public Image mapImage;
	public Image arrow;
	public Text mapName;
	public Transform playertr;
	public void UpdateMap()
    {
		mapName.text = Models.User.Instance.CurrentMapData.Name;
		mapImage.sprite = MiniMapManager.Instance.LoadCurrentMiniMap();
		mapImage.SetNativeSize();
		mapImage.transform.localPosition = Vector3.zero;
		this.boundary = MiniMapManager.Instance.Boundary;
		playertr = null;
	}
	void Start () {
		MiniMapManager.Instance.uIMiniMap = this;
	}
	void Update () {
        if (playertr == null)
        {
			if (MiniMapManager.Instance.PlayerTransform != null)
				playertr = MiniMapManager.Instance.PlayerTransform;
			else
				return;
		}
		if(boundary != null)
			UpdateMinimapLocation();
	}
	private void UpdateMinimapLocation()
    {
		float bxmin = boundary.bounds.min.x;
		float bymin = boundary.bounds.min.z;
		float relax = (playertr.position.x - bxmin) / boundary.bounds.size.x;
		float relay = (playertr.position.z - bymin) / boundary.bounds.size.z;
		mapImage.rectTransform.pivot = new Vector2(relax, relay);
		mapImage.transform.localPosition = Vector3.zero;
		arrow.transform.eulerAngles = new Vector3(0, 0, -playertr.eulerAngles.y);
    }
}
