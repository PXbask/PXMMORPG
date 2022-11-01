using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelector : MonoSingleton<TargetSelector> {

	Projector projector;
	private bool actived = false;
	Vector3 center;
	/// <summary>
	/// 技能释放范围
	/// </summary>
	private float range;
	/// <summary>
	/// 技能造成伤害范围
	/// </summary>
	private float size;
	Vector3 offset = new Vector3(0, 2, 0);

	protected Action<Vector3> selectPoint;
    protected override void OnAwake()
    {
        projector = GetComponentInChildren<Projector>();
		projector.gameObject.SetActive(actived);
    }
	public void Active(bool active)
    {
		this.actived = active;
		if (this.projector == null) return;
		projector.gameObject.SetActive(active);
		projector.orthographicSize = this.size / 2f;
	}
	private void Update()
    {
        if (!this.actived) return;
        if (projector == null) return;
        this.center = GameObjectTool.LogicToWorld(Models.User.Instance.CurrentCharacter.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 100f, LayerMask.GetMask("Terrain")))
        {
            Vector3 hitPoint = hitInfo.point;
            Vector3 dist = hitPoint - this.center;
            if (dist.magnitude > this.range)
            {
                hitPoint = this.center + dist.normalized * this.range;
            }
            this.projector.gameObject.transform.position = hitPoint + offset;
            if (Input.GetMouseButtonDown(0))
            {
                this.selectPoint(hitPoint);
                this.Active(false);
            }
            if (Input.GetMouseButtonDown(1))
            {
                this.Active(false);
            }
        }
    }
    private void LateUpdate()
    {
        if(this.actived)
            transform.Rotate(Vector3.up, 15f * Time.deltaTime, Space.Self);
    }
    public static void ShowSelector(int range, int size, Action<Vector3> onPositionSelected)
    {
        if (TargetSelector.Instance == null) return;
        TargetSelector.Instance.range = GameObjectTool.LogicToWorld(range);
        TargetSelector.Instance.size = GameObjectTool.LogicToWorld(size);
        TargetSelector.Instance.selectPoint = onPositionSelected;
        TargetSelector.Instance.Active(true);
    }
}
