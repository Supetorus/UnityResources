using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		other.gameObject.GetComponent<Renderer>().material.color = Color.red;
	}

	void OnTriggerExit(Collider other)
	{
		other.gameObject.GetComponent<Renderer>().material.color = Color.green;
	}
}
