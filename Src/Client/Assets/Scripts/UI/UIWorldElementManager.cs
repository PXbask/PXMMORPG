using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Manager;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{
	public GameObject uinameBar;
	public GameObject npcStatusPrefab;
    public GameObject uiPopupText;
	public Dictionary<Transform, GameObject> elementNames = new Dictionary<Transform, GameObject>();	
	public Dictionary<Transform, GameObject> elementStatus = new Dictionary<Transform, GameObject>();
    public Dictionary<Transform, GameObject> elementText = new Dictionary<Transform, GameObject>();
	
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
        if (!elementStatus.ContainsKey(tr))
        {
            GameObject go = Instantiate(npcStatusPrefab, transform);
            go.name = string.Format("npcStatusPrefab [{0}]", tr.name);
            go.GetComponent<UIWorldElement>().owner = tr;
            go.GetComponent<UIQuestStatus>().SetQuestStatus(status);
            go.SetActive(true);
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
    public void ShowPopupText(PopupType type,Vector3 position,float damage,bool isCrit)
    {
        GameObject goPopup = Instantiate(uiPopupText, position, Quaternion.identity, this.transform);
        goPopup.name = "Popup";
        goPopup.GetComponent<UIPopupText>().InitPopup(type, damage, isCrit);
        goPopup.SetActive(true);
    }
}
