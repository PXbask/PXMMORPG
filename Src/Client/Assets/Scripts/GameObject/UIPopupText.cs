using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum PopupType
{
	None,
	Damage,
	Heal
}
public class UIPopupText : MonoBehaviour {

	public Text normalDamText;
	public Text critDamText;
	public Text healText;
	public float floatTime = 0.5f;

	public void InitPopup(PopupType type,float number,bool isCrit)
    {
		string text = number.ToString("0");
		normalDamText.text = text;
		critDamText.text = text;
		healText.text = text;

		normalDamText.enabled = !isCrit && number < 0;
		critDamText.enabled = isCrit && number < 0;
		healText.enabled = number > 0;

		float time = Random.Range(0f, 0.5f) + floatTime;

		float height = Random.Range(0.5f, 1f);
		float disperse = Random.Range(-0.5f, 0.5f);
		disperse += Mathf.Sign(disperse) * 0.3f;

		LeanTween.moveX(this.gameObject, transform.position.x + disperse, time);
		LeanTween.moveZ(this.gameObject, transform.position.z + disperse, time);
		LeanTween.moveY(this.gameObject, transform.position.y + height, time).setEaseOutBack().setDestroyOnComplete(true);
    }
}
