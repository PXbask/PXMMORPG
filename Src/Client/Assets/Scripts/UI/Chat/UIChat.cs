using Candlelight.UI;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChat : MonoBehaviour {

    public HyperText textArea;
    public UITabView channelTab;
    public InputField chatText;
    public Text chatTarget;
    public Dropdown channelSelect;

    private void Start()
    {
        this.channelTab.OnTabSelect += OnDisplayChannelSelected;
        ChatManager.Instance.OnChat += RefreshUI;
    }
    private void OnDestroy()
    {
        this.channelTab.OnTabSelect += OnDisplayChannelSelected;
        ChatManager.Instance.OnChat += RefreshUI;
    }
    private void Update()
    {
        InputManager.Instance.isInputMode = chatText.isFocused;
    }
    private void OnDisplayChannelSelected(int idx)
    {
        ChatManager.Instance.displayChannel = (ChatManager.LocalChannel)idx;
        RefreshUI();
    }

    private void RefreshUI()
    {
        this.textArea.text = ChatManager.Instance.GetCurrentMessages();
        this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
        if(ChatManager.Instance.SendChannel == SkillBridge.Message.ChatChannel.Private)
        {
            this.chatTarget.gameObject.SetActive(true);
            if(ChatManager.Instance.PrivateID != 0)
            {
                this.chatText.text = ChatManager.Instance.PrivateName + ":";
            }
            else
            {
                this.chatText.text = "<无>:";
            }
        }
        else
        {
            this.chatTarget.gameObject.SetActive(false);
        }
    }
    public void OnClickChatLink(HyperText text,HyperText.LinkInfo link)
    {
        if (string.IsNullOrEmpty(link.Name))
            return;
        if (link.Name.StartsWith("c:"))
        {
            string[] strs = link.Name.Split(":".ToCharArray());
            UIPopChatMenu chatMenu = UIManager.Instance.Show<UIPopChatMenu>();
            chatMenu.targetId=int.Parse(strs[1]);
            chatMenu.targetName=strs[2];
        }
    }
    public void OnClickSend()
    {
        OnEndInput(this.chatText.text);
    }

    public void OnEndInput(string text)
    {
        if (!string.IsNullOrEmpty(text.Trim()))
            this.SendChat(text);
        this.chatText.text = string.Empty;
    }

    private void SendChat(string text)
    {
        ChatManager.Instance.SendChat(text, ChatManager.Instance.PrivateID, ChatManager.Instance.PrivateName);
    }
    public void OnSendChannelChanged(int idx)
    {
        if (ChatManager.Instance.sendChannel == (ChatManager.LocalChannel)(idx + 1))
            return;
        if(!ChatManager.Instance.SetSendChannel((ChatManager.LocalChannel)(idx + 1)))
        {
            this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
        }
        else
        {
            this.RefreshUI();
        }
    }
}
