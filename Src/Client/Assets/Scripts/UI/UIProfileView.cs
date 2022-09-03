﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProfileView : MonoBehaviour {
	public RectTransform rect_content;
	public GameObject uicharinfo;
	private RectTransform rectTransform;
	public UICharacterView characterView;
	public List<GameObject> profileButtons;
	public UnityEngine.Events.UnityAction<int> OnSelectCharacter;
	void Start () {
        rectTransform = uicharinfo.GetComponent<RectTransform>();
    }
	
	void Update () {

	}
	public void AddProfile(SkillBridge.Message.NCharacterInfo nCharacterInfo,int idx)
    {
		var sd = rectTransform.rect.height * uicharinfo.transform.lossyScale.x;
		rect_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect_content.rect.width);
		rect_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect_content.rect.height + sd);
		var go = Instantiate(uicharinfo, rect_content);
		UICharInfo info = go.GetComponent<UICharInfo>();
		info.info = nCharacterInfo;
		go.GetComponent<Button>().onClick.AddListener(() => OnSelectCharacter(idx));
		profileButtons.Add(go);
	}
	public void RemoveProfile()
    {
		
    }
	public void UpdateProfileData()
    {
		foreach (var c in profileButtons)
		{
			Destroy(c);
		}
		profileButtons.Clear();

		for (int i = 0; i < Models.User.Instance.Info.Player.Characters.Count; i++)
		{
			this.AddProfile(Models.User.Instance.Info.Player.Characters[i], i);
		}

		if(profileButtons.Count > 0)
        {
			characterView.CurrentCharacter = (int)profileButtons[0].GetComponent<UICharInfo>().info.Class - 1;
        }
        else
        {
			characterView.CurrentCharacter = -1;
        }
	}
}
