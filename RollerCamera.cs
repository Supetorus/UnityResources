using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerCamera : MonoBehaviour
{
	public Transform target;
	private float distance = 5;
	public float maxDistance = 5;
	public float pitch = 45;
	public float sensitivity = 1;

	float yaw = 0;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{
		yaw += Input.GetAxis("Mouse X") * sensitivity;
		pitch -= Input.GetAxis("Mouse Y") * sensitivity;
		Quaternion qYaw = Quaternion.AngleAxis(yaw, Vector3.up);
		Quaternion qPitch = Quaternion.AngleAxis(pitch, Vector3.right);
		Quaternion rotation = qYaw * qPitch;

		float checkDistance = maxDistance;
		while (Physics.Raycast(target.transform.position, transform.position - target.transform.position, checkDistance))
		{
			checkDistance -= 0.01f;
		}
		distance = checkDistance;

		Vector3 offset = rotation * Vector3.back * distance;

		transform.position = target.position + offset;
		transform.rotation = Quaternion.LookRotation(-offset, Vector3.up);
	}
}
