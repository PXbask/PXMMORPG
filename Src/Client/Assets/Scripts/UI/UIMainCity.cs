using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;
using Models;

public class UIMainCity : MonoBehaviour {
    public Text nameText;
    public Text level;
    private void Start()
    {
        UpdateInfo();
    }
	public void OnClickBack2Select()
    {

    }
    public void UpdateInfo()
    {
        this.nameText.text = User.Instance.CurrentCharacter.Name;
        this.level.text = User.Instance.CurrentCharacter.Level.ToString();
    }
}
