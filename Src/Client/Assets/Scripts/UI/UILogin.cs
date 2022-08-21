﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class UILogin : MonoBehaviour {
	public InputField username;
	public InputField password;
	public Button buttonlogin;
	public Toggle agreeToggle;
	void Start () {
		UserService.Instance.OnLogin = this.OnLogin;
		buttonlogin.onClick.AddListener(this.OnClickLogin);
	}
	
	void Update () {
		
	}
	void OnLogin(SkillBridge.Message.Result result, string msg)
	{
		MessageBox.Show(string.Format("结果：{0} msg:{1}", result, msg));
	}
	public void OnClickLogin()
    {
		if(string.IsNullOrEmpty(username.text))
        {
			MessageBox.Show("请输入用户名");
			return;
        }
        if (string.IsNullOrEmpty(password.text))
        {
			MessageBox.Show("请输入密码");
			return;
        }
		if (!this.agreeToggle.isOn)
		{
			MessageBox.Show("请先同意用户协议");
			return;
		}
		UserService.Instance.SendLogin(username.text, password.text);
    }
}
