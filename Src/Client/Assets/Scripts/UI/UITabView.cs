using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabView : MonoBehaviour {

    public UITabButton[] uITabButtons;
    public GameObject[] tabPages;
    public int index = -1;

    private IEnumerator Start()
    {
        for(int i = 0; i < tabPages.Length; i++)
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
            for(int j = 0; j < tabPages.Length; j++)
            {
                uITabButtons[j].DoSelect(j == tabIndex);
                tabPages[j].SetActive(j== tabIndex);
            }
            this.index = tabIndex;
        }
    }
}
