using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputWraper
{

	private static float s_TouchBeginTime = 0;
	
	public static bool GetInputLocationOnRect(RectTransform rect, out Vector2 tapPosition, out bool isHeld)
	{
		bool isClicked = Input.GetMouseButtonDown((int)MouseButton.Left) && Input.touchCount == 0;
		bool isRightClicked = Input.GetMouseButtonDown((int)MouseButton.Right);
		bool isTouch = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
		bool isTouchBegin = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
		isHeld = false;

		if (isTouchBegin)
			s_TouchBeginTime = Time.time;

		if (isRightClicked) isHeld = true;
		if (Input.touchCount > 0 && Time.time - s_TouchBeginTime >= 1.0f / 2.0f)
		{
			isHeld = true;
			s_TouchBeginTime = Time.time;
		}

		if (isClicked || isHeld || isTouch)
		{
			Vector2 touchPosition = isTouch ? Input.GetTouch(0).position :(Vector2)Input.mousePosition;
			Vector2 position;
			if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, touchPosition, null, out position))
			{
				if (position.x >= rect.rect.xMin && position.x <= rect.rect.xMax &&
					position.y >= rect.rect.yMin && position.y <= rect.rect.yMax)
				{
					position.x -= rect.rect.xMin;
					position.y = rect.rect.yMax - position.y;
					tapPosition = position;
					return true;
				}
			}
		}

		tapPosition = Vector2.zero;
		return false;
	}
}
