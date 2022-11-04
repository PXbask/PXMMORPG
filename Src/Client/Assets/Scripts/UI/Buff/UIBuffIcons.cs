using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuffIcons : MonoBehaviour {

	private Creature Owner;
	public GameObject itemPrefab;
	private Dictionary<int,GameObject> Buffs = new Dictionary<int,GameObject>();
	void Awake () {
		
	}
	void OnDestroy () {
		this.ClearBuffs();
	}
	public void SetOwner(Creature owner)
    {
		if(this.Owner != null && this.Owner != owner)
        {
			this.ClearBuffs();
		}
		this.Owner = owner;
		this.Owner.OnBuffAdd += OnBuffAdd;
		this.Owner.OnBuffRemove += OnBuffRemove;

		this.InitBuffs();
	}

    private void InitBuffs()
    {
        foreach (var kv in this.Owner.BuffMgr.Buffs)
        {
			this.OnBuffAdd(kv.Value);
        }
    }

    private void ClearBuffs()
    {
        if (this.Owner != null)
        {
			this.Owner.OnBuffAdd -= OnBuffAdd;
			this.Owner.OnBuffRemove -= OnBuffRemove;
		}

        foreach (var kv in this.Buffs)
        {
			Destroy(kv.Value);
        }
		this.Buffs.Clear();
	}

    private void OnBuffAdd(Battle.Buff buff)
    {
		GameObject go = Instantiate(itemPrefab, this.transform);
		go.name = buff.define.Id.ToString();
		UIBuffItem bi = go.GetComponent<UIBuffItem>();
		bi.SetItem(buff);
		go.SetActive(true);
		this.Buffs.Add(buff.buffId, go);
    }
	private void OnBuffRemove(Battle.Buff buff)
    {
		GameObject go;
		if(this.Buffs.TryGetValue(buff.buffId,out go))
        {
			this.Buffs.Remove(buff.buffId);
			Destroy(go);
        }
    }
}
