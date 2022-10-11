using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;
using Models;
using Manager;

public class UIMain : MonoSingleton<UIMain> {
    public Text nameText;
    public Text level;
    public UITeam TeamWindow;
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
    public void OnClickFriends()
    {
        Manager.UIManager.Instance.Show<UIFriends>();
    }
    public void ShowTeamUI(bool show)
    {
        TeamWindow.ShowTeam(show);
    }
    public void OnClickGuild()
    {
        GuildManager.Instance.ShowGuild();
    }
    public void OnClickRide()
    {
        Manager.UIManager.Instance.Show<UIRide>();
    }
    public void OnClickSetting()
    {
        UIManager.Instance.Show<UISetting>();
    }
    public void OnClickSkill()
    {

    }
    #endregion
}
