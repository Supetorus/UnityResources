using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
	[SerializeField] FloatData force;

	private void Update()
	{
		//print(force.value);
	}

	public void OnPause()
	{
		print("pause");
	}
}
