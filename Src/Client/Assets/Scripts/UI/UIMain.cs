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
    public UICreatureInfo targetUI;
    protected override void OnAwake()
    {
        UpdateInfo();
        this.targetUI.gameObject.SetActive(false);
        BattleManager.Instance.OnTargetChanged += OnTargetChanged;
    }

    private void OnTargetChanged(Creature creature)
    {
        if (creature != null)
        {
            if(!targetUI.isActiveAndEnabled) targetUI.gameObject.SetActive(true);
            targetUI.Target = creature;
        }
        else
        {
            targetUI.gameObject.SetActive(false);
        }
    }

    public void UpdateInfo()
    {
        this.nameText.text = User.Instance.CurrentCharacterInfo.Name;
        this.level.text = User.Instance.CurrentCharacterInfo.Level.ToString();
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
        UIManager.Instance.Show<UISkill>();
    }
    #endregion
}
