﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINameBar : MonoBehaviour {
	public Text nameText;
	public Entities.Character character;
	public UIBuffIcons buffIcons;
	// Use this for initialization
	void Start () {
		if (character != null)
        {
			UpdateInfo();
			buffIcons.SetOwner(character);
		}
	}
	
	// Update is called once per frame
	void Update () {
		this.UpdateInfo();
	}
	public void UpdateInfo()
    {
		this.nameText.text = string.Format("{0} Lv.{1}",character.Info.Name,character.Info.Level.ToString());
    }
}
