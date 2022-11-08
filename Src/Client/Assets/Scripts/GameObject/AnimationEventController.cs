using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour {

	public EntityEffectManager EffectMgr;
	private void PlayEffect(string name)
	{
		Debug.LogFormat("AnimationEventController: [{0}]PlayEffect[{1}]", this.name, name);
		EffectMgr.PlayEffect(name);
	}
	private void PlaySound(string name)
	{
		Debug.LogFormat("AnimationEventController: [{0}]PlaySound[{1}]", this.name, name);
	}
}
