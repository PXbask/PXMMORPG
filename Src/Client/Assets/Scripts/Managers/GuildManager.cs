﻿using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class GuildManager : Singleton<GuildManager>
    {
        internal NGuildInfo guildInfo;
        public NGuildMemberInfo myMemberInfo;
        public bool HasGuild
        {
            get { return guildInfo != null; }
        }

        public void Init(NGuildInfo guild)
        {
            this.guildInfo = guild;
            if (guild == null)
            {
                myMemberInfo = null;
                return;
            }
            else
            {
                foreach(var member in guild.Members)
                {
                    if (member.characterId == User.Instance.CurrentCharacterInfo.Id)
                    {
                        myMemberInfo = member;
                        break;
                    }
                }
            }
        }

        internal void ShowGuild()
        {
            if (this.HasGuild)
            {
                UIManager.Instance.Show<UIGuild>();
            }
            else
            {
                var win = UIManager.Instance.Show<UIGuildPopNoGuild>();
                win.OnClose += PopNoGuild_OnClose;
            }
        }

        private void PopNoGuild_OnClose(UIWindow sender, UIWindow.WindowResult result)
        {
            if(result == UIWindow.WindowResult.Yes)
            {
                UIManager.Instance.Show<UIGuildPopCreate>();
            }
            else if(result == UIWindow.WindowResult.No) 
            {
                UIManager.Instance.Show<UIGuildList>();
            }
        }
    }
}

