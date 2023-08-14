using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	[SerializeField] GameObject player;
	[SerializeField] Vector3 offset;
	[SerializeField] float smoothingSpeed;

	// Update is called once per frame
	void Update()
	{
		Vector3 desiredPosition = player.transform.position + (transform.rotation * offset);

		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothingSpeed * Time.deltaTime);
		transform.position = smoothedPosition;

		transform.LookAt(player.transform.position);
	}
}
