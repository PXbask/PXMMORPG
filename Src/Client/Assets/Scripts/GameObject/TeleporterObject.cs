using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterObject : MonoBehaviour
{
	public int ID;
	private Mesh mesh = null;
	// Use this for initialization
	void Start()
	{
		mesh = GetComponent<MeshFilter>().sharedMesh;
	}

	// Update is called once per frame
	void Update()
	{

	}
	void OnTriggerEnter(Collider other)
	{
		PlayerInputController controller = other.GetComponent<PlayerInputController>();
		if (controller != null && controller.isActiveAndEnabled)
		{
			TeleporterDefine define = Manager.DataManager.Instance.Teleporters[ID];
			if (define == null)
			{
				Debug.LogErrorFormat("TeleporterObject: Character[{0}] EnterTeleporter[{1}] ,ButTeleporterDefine isn't Existed", controller.character.Info.Name, ID);
				return;
			}
			Debug.LogFormat("TeleporterObject: Character[{0}] EnterTeleporter[{1}:{2}]", controller.character.Info.Name, define.ID, define.Name);
			if (define.LinkTo > 0)
			{
				if (Manager.DataManager.Instance.Teleporters.ContainsKey(define.LinkTo))
				{
					Services.MapService.Instance.SendMapTeleport(ID);
				}
				else
				{
					Debug.LogErrorFormat("Teleporter ID:{0} LinkID:{1} error!", define.ID, define.LinkTo);
				}
			}
		}
	}
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		if (this.mesh != null)
		{
			Gizmos.DrawWireMesh(this.mesh, this.transform.position + Vector3.up * this.transform.localScale.y * .5f, transform.rotation, transform.localScale);
		}
		UnityEditor.Handles.color = Color.red;
		UnityEditor.Handles.ArrowHandleCap(0, transform.position, transform.rotation, 2f, EventType.Repaint);
	}
#endif
}
