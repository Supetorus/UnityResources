using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RButton : MonoBehaviour
{
	static Vector2 chipSize = new Vector2(100, 100);
	Vector2 size = Vector2.zero;
	Image image;
	RectTransform rect;
	public int chip = -1;

	void Start()
	{
		image = GetComponent<Image>();
		rect = GetComponent<RectTransform>();
	}

	public void SetChip(bool set, Sprite chip)
	{
		if(set)
		{
			if (size == Vector2.zero) { size = rect.sizeDelta; }
			rect.sizeDelta = chipSize;
			image.sprite = chip;
			image.color = Color.white;
			image.raycastPadding = new Vector4((size.x - chipSize.x) * -0.5f, (size.y - chipSize.y) * -0.5f, (size.x - chipSize.x) * -0.5f, (size.y - chipSize.y) * -0.5f);
		}
		else
		{
			if (size != Vector2.zero) { rect.sizeDelta = size; }
			image.sprite = null;
			image.color = Color.clear;
			image.raycastPadding = Vector4.zero;
		}
	}
}
