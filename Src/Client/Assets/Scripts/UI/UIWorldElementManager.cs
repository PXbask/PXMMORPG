using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{
	public GameObject uinameBar;
	public Dictionary<Transform, GameObject> worldElements = new Dictionary<Transform, GameObject>();	
	
	public void AddCharacterNameBar(Transform tr, Character character)
    {
		GameObject go = Instantiate(uinameBar, transform);
		go.name = string.Format("Character[{0}]Bar", character.entityId);
        go.GetComponent<UIWorldElement>().owner = tr;
        go.GetComponent<UINameBar>().character = character;
        if (!worldElements.ContainsKey(tr))
        {
			worldElements.Add(tr, go);
        }
    }
	public void RemoveElement(Transform tr)
    {
        if (worldElements.ContainsKey(tr))
        {
            Destroy(worldElements[tr]);
            worldElements.Remove(tr);
        }
    }
}
