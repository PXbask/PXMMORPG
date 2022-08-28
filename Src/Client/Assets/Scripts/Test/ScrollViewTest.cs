using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScrollViewTest : MonoBehaviour {

	RectTransform rectTransform;
	void Start () {
		rectTransform = GetComponent<RectTransform>();	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("-----------------");
			Debug.Log("x:"+rectTransform.rect.x);
			Debug.Log("xMin:" + rectTransform.rect.xMin);
			Debug.Log("xMax:" + rectTransform.rect.xMax);
			Debug.Log("y:" + rectTransform.rect.y);
			Debug.Log("yMin:" + rectTransform.rect.yMin);
			Debug.Log("yMax:" + rectTransform.rect.yMax);
			Debug.Log("min:" + rectTransform.rect.min);
			Debug.Log("max:" + rectTransform.rect.max);
			Debug.Log("size:" + rectTransform.rect.size);
			Debug.Log("sizeDelta:" + rectTransform.sizeDelta);
		}
        if (Input.GetMouseButtonDown(1))
        {
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500);
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500);
		}
	}
}
