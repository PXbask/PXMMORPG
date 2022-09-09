using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

	public Collider boundary;
	// Use this for initialization
	void Start () {
		Manager.MiniMapManager.Instance.UpdateBoundary(boundary);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
