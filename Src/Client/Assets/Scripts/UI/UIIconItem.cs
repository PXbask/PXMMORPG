using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIconItem : MonoBehaviour {
    public Image MainImage;
    public Image SecondImage;

    public Text MainText;

    public void SetMainIcon(string iconName,string text)
    {
        this.MainText.text = text;
        this.MainImage.sprite = Resloader.Load<Sprite>(iconName);
    }
}
