using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UICharInfoType
{
    Normal,
    Create
}
public class UICharInfo : MonoBehaviour {


    public SkillBridge.Message.NCharacterInfo info;

    public Text charClass;
    public Text charName;
    public Image highlight;
    public Image rightImage;

    public Sprite normal_Sprite;
    public Sprite create_Sprite;

    private UICharInfoType infoType;
    public UICharInfoType InfoType
    {
        get { return infoType; }
        set
        {
            infoType = value; 
            rightImage.sprite=(infoType==UICharInfoType.Normal)?normal_Sprite:create_Sprite;
        }
    }
    public bool Selected
    {
        get { return highlight.IsActive(); }
        set
        {
            highlight.gameObject.SetActive(value);
        }
    }

    // Use this for initialization
    void Start () {
        InfoType = UICharInfoType.Normal;
        highlight.gameObject.SetActive(false);
        if (info!=null)
        {
            this.charClass.text = this.info.Class.ToString();
            this.charName.text = this.info.Name;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
