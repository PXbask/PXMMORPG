using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniMap : MonoBehaviour {
	public BoxCollider boundary;
	public Image mapImage;
	public Image arrow;
	public Text mapName;
	public Transform playertr;
	private void InitMap()
    {
		mapName.text = Models.User.Instance.CurrentMapData.Name;
		mapImage.sprite = MiniMapManager.Instance.LoadCurrentMiniMap();
		mapImage.SetNativeSize();
		mapImage.transform.localPosition = Vector3.zero;
	}
	void Start () {
		InitMap();
	}
	void Update () {
        if (playertr == null)
        {
			if (Models.User.Instance.CurrentCharacterObject != null)
				playertr = Models.User.Instance.CurrentCharacterObject.transform;
			else
				return;
		}
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
