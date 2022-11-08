using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEffectManager : MonoBehaviour {

	public Transform root;
	private Dictionary<string,GameObject> effects = new Dictionary<string,GameObject>();
	public Transform[] props;

	void Start()
    {
		this.effects.Clear();
        if (root.childCount > 0)
        {
            for (int i = 0; i < root.childCount; i++)
            {
                this.effects.Add(root.GetChild(i).name, root.GetChild(i).gameObject);
            }
        }
        if(props != null)
        {
            for(int i=0; i < props.Length; i++)
            {
                this.effects.Add(this.props[i].name, props[i].gameObject);
            }
        }
    }
    public void PlayEffect(string name)
    {
        Debug.LogFormat("[{0}]PlayEffect:[{1}]", this.name, name);
        if (this.effects.ContainsKey(name))
        {
            this.effects[name].SetActive(true);
        }
    }

    public void PlayEffect(EffectType type, string name, Transform target, float duration)
    {
        if (type.Equals(EffectType.Bullet))
        {
            EffectController effect = InstantiateEffect(name);
            if(effect != null)
            {
                effect.Init(type, this.transform, target, duration);
                effect.gameObject.SetActive(true);
            }
        }
        else
        {
            PlayEffect(name);
        }
    }

    private EffectController InstantiateEffect(string name)
    {
        GameObject prefab;
        if(this.effects.TryGetValue(name, out prefab))
        {
            GameObject go = Instantiate(prefab, GameObjectManager.Instance.transform, true);
            go.transform.position = prefab.transform.position;
            go.transform.rotation = prefab.transform.rotation;
            return go.GetComponent<EffectController>();
        }
        return null;
    }
}
