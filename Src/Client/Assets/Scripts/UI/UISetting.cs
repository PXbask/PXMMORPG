using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : UIWindow {

    public void OnClickBackSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        Services.UserService.Instance.SendGameLeave();
        this.OnCloseClick();
    }
    public void OnClickExitGame()
    {
        Services.UserService.Instance.SendGameLeave(isQuitGame: true);
    }
}
