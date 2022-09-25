﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UITabView : MonoBehaviour {

    public UITabButton[] uITabButtons;
    public GameObject[] tabPages;
    public UnityAction<int> OnTabSelect;
    public int index = -1;

    private IEnumerator Start()
    {
        for(int i = 0; i < uITabButtons.Length; i++)
        {
            uITabButtons[i].tabView = this;
            uITabButtons[i].tabIndex = i;
        }
        yield return new WaitForEndOfFrame();
        SelectTab(0);
    }

    internal void SelectTab(int tabIndex)
    {
        if (this.index != tabIndex)
        {
            for(int j = 0; j < uITabButtons.Length; j++)
            {
                uITabButtons[j].DoSelect(j == tabIndex);
                if (j < tabPages.Length)
                    tabPages[j].SetActive(j == tabIndex);
            }
            this.index = tabIndex;
            if(this.OnTabSelect != null)
                this.OnTabSelect(tabIndex);
        }
    }
}
