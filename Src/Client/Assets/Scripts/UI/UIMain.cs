using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;
using Models;

public class UIMain : MonoSingleton<UIMain> {
    public Text nameText;
    public Text level;
    protected override void OnAwake()
    {
        UpdateInfo();
    }
    public void UpdateInfo()
    {
        this.nameText.text = User.Instance.CurrentCharacter.Name;
        this.level.text = User.Instance.CurrentCharacter.Level.ToString();
    }
    #region Event
    public void OnClickBack2Select()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        Services.UserService.Instance.SendGameLeave();
    }
    public void OpenBag()
    {
        Manager.UIManager.Instance.Show<UIBag>();
    }
    public void OnClickCharEquip()
    {
        Manager.UIManager.Instance.Show<UICharEquip>();
    }
    public void OnClickQuest()
    {
        Manager.UIManager.Instance.Show<UIQuestSystem>();
    }
    #endregion
}
