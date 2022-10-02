using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager{

    public class TeamManager : Singleton<TeamManager>
    {
        public void Init() { }
        internal void UpdateTeamInfo(NTeamInfo team)
        {
            Models.User.Instance.TeamInfo = team;
            ShowTeamUI(team != null);
        }

        private void ShowTeamUI(bool show)
        {
            if(UIMain.Instance != null)
            {
                UIMain.Instance.ShowTeamUI(show);
            }
        }
    }
}
