using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
using Models;
using Services;
using Manager;
public class UICharacterSelect : MonoBehaviour {

	public UICharacterView characterView;
	public GameObject panelSelect;
	public GameObject panelCreate;
	public UIProfileView uiprofileView;

	public InputField nameField;
	public GameObject[] classTitles;
	public GameObject[] classButtons;
	public GameObject[] classButtons_Select;

	public Text descripText;

	private int selectCharacterIdx = -1;
	private CharacterClass charClass;
	// Use this for initialization
	void Start () {
		UserService.Instance.OnCreateChar = this.OnCreateChar;
		uiprofileView=GameObject.Find("Canvas/CharSelect/ProfileButtons").GetComponent<UIProfileView>();
		uiprofileView.OnSelectCharacter = this.OnSelectCharacter;
		#if UNITY_EDITOR
		DataManager.Instance.Load();
		#endif
		InitCharacterSelect(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void InitCharacterSelect(bool init)
    {
		panelSelect.SetActive(true);
		panelCreate.SetActive(false);
        if (init)
        {
			uiprofileView.UpdateProfileData();
        }
    }
	public void InitCharacterCreate()
    {
		panelCreate.SetActive(true);
		panelSelect.SetActive(false);
		OnSelectClass(1);//select Warrior as default
	}
	/// <summary>
	/// 其实是Create的逻辑
	/// </summary>
	/// <param name="charClass"></param>
	public void OnSelectClass(int charClass)
    {
		this.charClass = (CharacterClass)charClass;
		characterView.CurrentCharacter = charClass - 1;
		for(int i = 0; i < 3; i++)
        {
			classTitles[i].SetActive(i == charClass - 1);
			classButtons[i].SetActive(!(i == charClass - 1));
			classButtons_Select[i].SetActive((i == charClass - 1));
		}
		descripText.text = DataManager.Instance.Characters[charClass].Description;
    }
	public void OnClickStartGame()
    {
        if (selectCharacterIdx >= 0)
        {
			UserService.Instance.SendGameEnter(selectCharacterIdx);
        }
    }
	public void OnClickSelectBackButton()
    {
		SceneManager.Instance.LoadScene("Loading");
    }
	/// <summary>
	/// 由Select进入Create界面逻辑
	/// </summary>
	public void OnEnterCreate()
	{
		InitCharacterCreate();
	}
	public void OnClickCreate()
    {
        if (string.IsNullOrEmpty(nameField.text))
        {
			MessageBox.Show("请输入角色名称");
			return;
        }
		UserService.Instance.SendCharCreate(nameField.text,charClass);
    }
	public void OnCreateChar(Result result, string msg)
    {
        if (result.Equals(Result.Success))
        {
			InitCharacterSelect(true);
        }
        else
        {
			MessageBox.Show(string.Format("结果：{0} msg:{1}", result, msg));
		}
    }
	public void OnSelectCharacter(int idx)
	{
		this.selectCharacterIdx = idx;
		var cha = User.Instance.Info.Player.Characters[idx];
		Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class);
		characterView.CurrentCharacter = (int)cha.Class - 1;

		for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
		{
			UICharInfo ci = uiprofileView.profileButtons[i].GetComponent<UICharInfo>();
			ci.Selected = selectCharacterIdx == i;
		}
	}
}
