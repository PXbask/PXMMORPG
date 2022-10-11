using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerCamera : MonoSingleton<MainPlayerCamera>
{
    public new Camera camera;
    public Transform viewPoint;

    public GameObject player;

    private void LateUpdate()
    {
        if (player == null)
        {
            if (Models.User.Instance.CurrentCharacterObject != null)
                player = Models.User.Instance.CurrentCharacterObject.gameObject;
            else
                return;
        }

        this.transform.position = player.transform.position;
        this.transform.rotation = player.transform.rotation;
    }
}
