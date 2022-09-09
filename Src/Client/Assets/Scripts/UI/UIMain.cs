using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;
using Models;

public class UIMain : MonoSingleton<UIMain> {
    public Text nameText;
    public Text level;
    protected override void OnStart()
    {
        UpdateInfo();
    }
	public void OnClickBack2Select()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        Services.UserService.Instance.SendGameLeave();
    }
    public void UpdateInfo()
    {
        this.nameText.text = User.Instance.CurrentCharacter.Name;
        this.level.text = User.Instance.CurrentCharacter.Level.ToString();
    }
    #region Test
    public void OpenUITest()
    {
        UITest uITest = Manager.UIManager.Instance.Show<UITest>();
        uITest.SetInfo("Title", "Content");
    }
    #endregion
}
