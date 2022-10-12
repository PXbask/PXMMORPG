﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class UIRegister : MonoBehaviour {

    public InputField username;
    public InputField password;
    public InputField passwordConfirm;
    public Button buttonRegister;
    public Toggle agreeToggle;

    // Use this for initialization
    void Start () {
        UserService.Instance.OnRegister = this.OnRegister;
        buttonRegister.onClick.AddListener(OnClickRegister);
    }
	
    void OnRegister(SkillBridge.Message.Result result, string msg)
    {
        if (result.Equals(SkillBridge.Message.Result.Success))
        {
            SceneManager.Instance.LoadScene("CharSelect");
        }
        else
        {
            MessageBox.Show(string.Format("结果：{0} msg:{1}", result, msg));
        }
    }
    // Update is called once per frame
    void Update () {
		
	}

    public void OnClickRegister()
    {
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
        if (string.IsNullOrEmpty(this.passwordConfirm.text))
        {
            MessageBox.Show("请输入确认密码");
            return;
        }
        if (this.password.text != this.passwordConfirm.text)
        {
            MessageBox.Show("两次输入的密码不一致");
            return;
        }
        if (!this.agreeToggle.isOn)
        {
            MessageBox.Show("请先同意用户协议");
            return;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        UserService.Instance.SendRegister(this.username.text, this.password.text);
    }
}
