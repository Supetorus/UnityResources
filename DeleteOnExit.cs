using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnExit : MonoBehaviour
{
	[SerializeField] float delay = 0;
	[SerializeField] GameObject toBeDeleted;

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player") Destroy(toBeDeleted, delay);
	}
}
