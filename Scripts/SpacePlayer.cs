using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePlayer : MonoBehaviour, IDestructable
{
	//Range sets the range that speed can be set to in the inspector.
	//Tooltip creates a tooltip in the inspector.
	[Range(0, 200)] [Tooltip("speed of the player")] public float speed = 100;

	// Update is called once per frame
	void Update()
	{
		// Movement
		Vector3 direction = Vector3.zero;
		direction.x = Input.GetAxis("Horizontal");
		direction.z = Input.GetAxis("Vertical");
		transform.Translate(direction * speed * Time.deltaTime);
		float x, z;
		x = transform.position.x < -190 ? -190 : transform.position.x;
		x = transform.position.x > 190 ? 190 : x;
		z = transform.position.z < -105 ? -105 : transform.position.z;
		z = transform.position.z > 105 ? 105 : z;
		transform.position = new Vector3(x, transform.position.y, z);

		// Shooting
		if ((Input.GetButton("Fire1")))
		{
			GetComponent<SpaceWeapon>().Fire();
		}

		// Health Bar
		GameManager.Instance.playerHealth = GetComponent<Health>().health;
	}

	public void Destroyed()
	{
		GameManager.Instance.playerHealth = 0;
		GameManager.Instance.OnPlayerDead();
	}
}
