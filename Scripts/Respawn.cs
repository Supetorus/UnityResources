using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
	[SerializeField] Transform respawnPoint;
	private void OnCollisionEnter(Collision collision)
	{
		collision.gameObject.transform.position = respawnPoint.position;
		collision.rigidbody.velocity = Vector3.zero;
	}
}
