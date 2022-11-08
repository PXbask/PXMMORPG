using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EffectType
{
	None,
	Bullet,
	Position,
	Hit,
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
	public void Init(EffectType type,Transform source,Transform target,Vector3 offset,float duration)
    {
		this.type = type;
		this.target = target;
		if (duration > 0)
			this.lifeTime = duration;
		this.time = 0;
        if (type.Equals(EffectType.Bullet))
        {
			this.startPos = this.transform.position;
			this.offset = offset;
			this.targetPos = target.position + offset;
		}
		else if (type.Equals(EffectType.Hit))
        {
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
			if(this.lifeTime > 0 && this.time >= this.lifeTime)
            {
				Destroy(this.gameObject);
				return;
			}
			this.transform.position = Vector3.Lerp(this.transform.position, this.targetPos, Time.deltaTime / (this.lifeTime - this.time));
        }
    }
}
