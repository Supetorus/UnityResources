using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
	public CinemachineVirtualCamera[] cams;
	int camIndex = 0;

	private void Start()
	{
		foreach (var cam in cams) cam.Priority = 0;
		cams[camIndex].Priority = 100;
	}

	public void NextCam(InputAction.CallbackContext context)
	{
		if (Game.Instance.IsPaused || !context.performed) return;

		cams[camIndex].Priority = 0;
		cams[camIndex].GetComponentInChildren<AimClickTarget>()?.gameObject.SetActive(false);
		camIndex = (camIndex + 1) % cams.Length;
		cams[camIndex].Priority = 100;
		cams[camIndex].GetComponentInChildren<AimClickTarget>()?.gameObject.SetActive(true);
	}
}
