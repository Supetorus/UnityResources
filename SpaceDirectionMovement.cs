using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceDirectionMovement : MonoBehaviour
{
	[SerializeField] Vector3 direction;
	[SerializeField] float speed;
	[SerializeField] Space space = Space.Self;

	void Update()
	{
		// Translate uses local space.
		transform.Translate(direction * speed * Time.deltaTime, space);
	}
}
