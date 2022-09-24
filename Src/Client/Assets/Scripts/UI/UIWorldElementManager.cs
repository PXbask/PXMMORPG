using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Manager;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{
	public GameObject uinameBar;
	public GameObject npcStatusPrefab;
	public Dictionary<Transform, GameObject> elementNames = new Dictionary<Transform, GameObject>();	
	public Dictionary<Transform, GameObject> elementStatus = new Dictionary<Transform, GameObject>();	
	
	public void AddCharacterNameBar(Transform tr, Character character)
    {
		GameObject go = Instantiate(uinameBar, transform);
		go.name = string.Format("Character[{0}]Bar", character.entityId);
        go.GetComponent<UIWorldElement>().owner = tr;
        go.GetComponent<UINameBar>().character = character;
        go.SetActive(true);
        if (!elementNames.ContainsKey(tr))
        {
			elementNames.Add(tr, go);
        }
    }
	public void RemoveCharacterNameBar(Transform tr)
    {
        if (elementNames.ContainsKey(tr))
        {
            Destroy(elementNames[tr]);
            elementNames.Remove(tr);
        }
    }
    public void AddNpcQuestStatus(Transform tr, NpcQuestStatus status)
    {
        if (this.elementStatus.ContainsKey(tr))
        {
            elementStatus[tr].GetComponent<UIQuestStatus>().SetQuestStatus(status);
        }
		GameObject go = Instantiate(npcStatusPrefab, transform);
        go.name = string.Format("npcStatusPrefab [{0}]", tr.name);
        go.GetComponent<UIWorldElement>().owner = tr;
        go.GetComponent<UIQuestStatus>().SetQuestStatus(status);
        go.SetActive(true);
        if (!elementStatus.ContainsKey(tr))
        {
            elementStatus.Add(tr, go);
        }
    }
	public void RemoveNpcQuestStatus(Transform tr)
    {
        if (elementStatus.ContainsKey(tr))
        {
            Destroy(elementStatus[tr]);
            elementStatus.Remove(tr);
        }
    }
}
