using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSound : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		GetComponent<AudioSource>().Play();
	}
}
