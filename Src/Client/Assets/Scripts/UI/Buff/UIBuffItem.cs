using Battle;
using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuffItem : MonoBehaviour {
	public Image icon;
	public Image overlay;
	public Text cdText;
	public Buff buff;
	void Start()
	{
		overlay.enabled = false;
		cdText.enabled = false;
    }
    void Update()
    {
        if (this.buff == null) return;
        if (this.buff.time > 0)
        {
            if (!overlay.enabled) overlay.enabled = true;
            if (!cdText.enabled) cdText.enabled = true;
            overlay.fillAmount = this.buff.define.CD / this.buff.define.Duration;
            cdText.text = ((int)Math.Ceiling(this.buff.define.Duration - this.buff.time)).ToString();
        }
        else
        {
            if (overlay.enabled) overlay.enabled = false;
            if (cdText.enabled) cdText.enabled = false;
        }
    }
    public void SetItem(Buff buff)
    {
        this.buff = buff;
        if (this.icon != null)
        {
            this.icon.overrideSprite = Resloader.Load<Sprite>(this.buff.define.Icon);
            this.icon.SetAllDirty();
        }
    }
}
