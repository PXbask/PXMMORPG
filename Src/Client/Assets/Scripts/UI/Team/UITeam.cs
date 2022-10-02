using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeam : MonoBehaviour {
    private const int MAX_MEMBER = 5;
    public Text teamTitle;
    public UITeamItem[] Members;
    public ListView listMain;

    private void Start()
    {
        if (User.Instance.TeamInfo == null)
        {
            gameObject.SetActive(false);
            return;
        }
        foreach(var item in Members)
        {
            this.listMain.AddItem(item);
        }
    }
    private void OnEnable()
    {
        UpdateTeamUI();
    }
    internal void ShowTeam(bool show)
    {
        this.gameObject.SetActive(show);
        if (show) UpdateTeamUI();
    }
    private void UpdateTeamUI()
    {
        if(User.Instance.TeamInfo == null) return;
        this.teamTitle.text = String.Format("我的队伍（{0}/5）",User.Instance.TeamInfo.Members.Count);
        for (int i = 0; i < MAX_MEMBER; i++)
        {
            if(i< User.Instance.TeamInfo.Members.Count)
            {
                this.Members[i].gameObject.SetActive(true);
                this.Members[i].SetMemberInfo(i, User.Instance.TeamInfo.Members[i], User.Instance.TeamInfo.Members[i].Id == User.Instance.TeamInfo.Leader);
            }
            else
            {
                this.Members[i].gameObject.SetActive(false);
            }
        }
    }
    public void OnClickLeave()
    {
        MessageBox.Show("确认要离开队伍吗？", "退出队伍", MessageBoxType.Confirm, "确定离开", "取消")
            .OnYes = () => { Services.TeamService.Instance.SendTeamLeaveRequest(User.Instance.TeamInfo.Id); };
    }

}
