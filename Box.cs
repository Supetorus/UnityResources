using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
	void OnCollisionEnter(Collision collision)
	{
		GetComponent<AudioSource>().Play();
	}
}
