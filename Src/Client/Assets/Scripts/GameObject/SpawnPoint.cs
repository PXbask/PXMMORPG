using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnPoint : MonoBehaviour {

	public int ID;
	private Mesh mesh = null;
	// Use this for initialization
	void Start()
	{
		mesh = GetComponent<MeshFilter>().sharedMesh;
	}
	void Update() { }
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Vector3 pos = this.transform.position;
		Gizmos.color = Color.red;
		if (this.mesh != null)
			Gizmos.DrawWireMesh(this.mesh, pos, this.transform.rotation, this.transform.localScale);
		UnityEditor.Handles.color = Color.red;
		UnityEditor.Handles.ArrowHandleCap(0, pos, this.transform.rotation, 1f,EventType.Repaint);
		UnityEditor.Handles.Label(pos, "SpawnPoint:" + this.ID);
	}
#endif
}
