using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoSingleton<FXManager> {

	public GameObject[] prefabs;
	private Dictionary<string, GameObject> Effects = new Dictionary<string, GameObject>();

    protected override void OnAwake()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            prefabs[i].SetActive(false);
            this.Effects.Add(prefabs[i].name, prefabs[i]);
        }
    }
    EffectController CreateEffect(string name,Vector3 pos)
    {
        GameObject prefab;
        if(this.Effects.TryGetValue(name, out prefab))
        {
            GameObject go = Instantiate(prefab,FXManager.Instance.transform,true);
            go.transform.position = pos;
            return go.GetComponent<EffectController>();
        }
        return null;
    }
    internal void PlayEffect(EffectType type,string name,Transform target,Vector3 pos,float duration)
    {
        EffectController effect = FXManager.Instance.CreateEffect(name, pos);
        effect.Init(type, this.transform, target, pos, duration);
        effect.gameObject.SetActive(true);
    }
}
