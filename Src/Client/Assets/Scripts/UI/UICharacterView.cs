using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour {

    public GameObject[] characterModels;
    private int currentCharacter;

    public int CurrentCharacter
    {
        get
        { return currentCharacter; }
        set
        {
            currentCharacter = value;
            UpdateCharacter();
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void UpdateCharacter()
    {
        for(int i = 0; i < characterModels.Length; i++)
        {
            characterModels[i].SetActive(i==CurrentCharacter);
        }
    }
}
