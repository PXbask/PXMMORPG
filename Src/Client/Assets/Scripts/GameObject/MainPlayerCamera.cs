using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerCamera : MonoSingleton<MainPlayerCamera>
{
    public new Camera camera;
    public Transform viewPoint;

    public GameObject player;
	// Use this for initialization
	protected override void OnStart () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        if (player == null)
            player = Models.User.Instance.CurrentCharacterObject;

        this.transform.position = player.transform.position;
        this.transform.rotation = player.transform.rotation;
    }
}
