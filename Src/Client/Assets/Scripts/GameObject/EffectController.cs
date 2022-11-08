﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EffectType
{
	None,
	Bullet,
}
public class EffectController : MonoBehaviour {

	public float lifeTime = 1f;
	float time = 0f;

	EffectType type;
	Transform target;
	Vector3 targetPos;
	Vector3 startPos;
	Vector3 offset;

	void OnEnable()
    {
        if (!type.Equals(EffectType.Bullet))
        {
			StartCoroutine(Run());
        }
    }
	IEnumerator Run()
    {
		yield return new WaitForSeconds(lifeTime);
		this.gameObject.SetActive(false);
    }
	public void Init(EffectType type,Transform source,Transform target,float duration)
    {
		this.type = type;
		this.target = target;
		this.lifeTime = duration;
        if (type.Equals(EffectType.Bullet))
        {
			this.startPos = this.transform.position;
			this.offset = new Vector3(0, this.transform.position.y - source.position.y, 0);
			this.targetPos = target.position + offset;
		}
    }
	void Update()
    {
        if (type.Equals(EffectType.Bullet))
        {
			this.time += Time.deltaTime;
            if (this.target != null)
            {
				this.targetPos = this.target.position + offset;
            }
			this.transform.LookAt(this.targetPos);
			if(Vector3.Distance(this.targetPos,transform.position) < 0.5f)
            {
				Destroy(this.gameObject);
				return;
            }
			this.transform.position = Vector3.Lerp(this.transform.position, this.targetPos, Time.deltaTime / (this.lifeTime - this.time));
        }
    }
}
