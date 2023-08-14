using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffector : MonoBehaviour
{
	[SerializeField][Range(-10, 10)] float strength = 5;
	public Water water = null;

	private float currentStrength;

	void Update()
	{
		if (Input.GetButtonDown("Fire1")) currentStrength = strength;
		if (Input.GetButton("Fire1"))
		{
			currentStrength *= 1.01f;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			water.Touch(ray, currentStrength);
		}
	}
}
