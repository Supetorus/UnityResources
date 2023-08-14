using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNext : MonoBehaviour
{
	[SerializeField] Transform[] edges;
	[SerializeField] GameObject[] possibleTransitions;
	private void OnTriggerEnter(Collider other)
	{
		foreach (var edge in edges)
		{
			var tile = Instantiate(possibleTransitions[Random.Range(0, possibleTransitions.Length)], edge.transform.position, edge.transform.rotation);
			Destroy(tile, 2.5f);
		}
	}
}
