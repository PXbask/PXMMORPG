using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UITest : UIWindow {
	public Text titleText;
	public Text contentText;
	protected override void OnStart()
    {
        this.OnClose += UITest_Event;
    }

    private void UITest_Event(UIWindow sender, WindowResult result)
    {
        MessageBox.Show(sender.name);
    }

    public void SetInfo(string title, string content)
	{
		titleText.text = title;
		contentText.text = content;
	}
}
