using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicTarget : MonoBehaviour
{
	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}
}
