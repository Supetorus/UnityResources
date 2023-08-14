using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource), typeof(Trajectory))]
public class Shooter : MonoBehaviour
{
	[Header("Shooting")]
	public AudioClip[] fireSounds;
	public GameObject projectile;
	public Transform spawnLocation;
	public float power;
	public float maxPower;
	public float shootFrequency = 1;
	[Header("Rotation")]
	[SerializeField, Tooltip("How quickly the canon rotates around the center of the map.")]
	private float rotationSpeed;
	[SerializeField, Tooltip("How quickly the canon turns left and right.")]
	private float horizontalSpeed;
	[SerializeField, Tooltip("How quickly the canon turns up and down.")]
	private float verticalSpeed;
	public float topXAngle = 315;   // 360 - 315 = 45 degrees up
	public float bottomXAngle = 15; // 15 degrees down
	public float minYAngle = 315;   // 360 - 315 = 45 degrees left
	public float maxYAngle = 45;    // 45 degrees right
	public GameObject globalPivot;
	public GameObject localPivot;
	public GameObject UpDownAimPivot;
	[Header("Mouse Aiming")]
	public LayerMask aimTarget;
	[Space]
	public new Camera camera;

	private Trajectory trajectory;
	private AudioSource audioSource;
	private bool canMoveOrShoot = true;
	private float timeSinceLastShot = 1;
	private List<GameObject> firedProjectiles = new List<GameObject>();

	public bool developerMode = false;

	private void Start()
	{
		trajectory = GetComponent<Trajectory>();
		audioSource = GetComponent<AudioSource>();
		pitch = transform.localRotation.eulerAngles.y;

	}

	private void Update()
	{
		timeSinceLastShot += Time.deltaTime;

		GetPlayerInput();
		trajectory.CalculateTrajectory(spawnLocation.position, spawnLocation.forward * power / projectile.GetComponent<Rigidbody>().mass);
	}

	internal void Reset()
	{
		foreach (GameObject projectile in firedProjectiles) Destroy(projectile);
		firedProjectiles.Clear();
	}

	private Vector2 aim = Vector2.zero;
	private float move = 0;

	private float pitch;
	private float yaw;

	private void GetPlayerInput()
	{
		if (!canMoveOrShoot || Game.Instance.IsPaused) return;


		pitch += -aim.y * verticalSpeed;
		yaw += aim.x * horizontalSpeed;

		pitch = Mathf.Clamp(pitch, -70, 10);
		yaw = Mathf.Clamp(yaw, -30, 30);

		localPivot.transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
		globalPivot.transform.Rotate(0, move * rotationSpeed, 0, Space.World);
	}

	public void Aim(InputAction.CallbackContext context)
	{
		aim = context.ReadValue<Vector2>();
	}

	public void Move(InputAction.CallbackContext context)
	{
		move = context.ReadValue<float>();
	}

	public void Pause(InputAction.CallbackContext context)
	{
		Game.Instance.TogglePause();
	}

	public void Fire(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		if (timeSinceLastShot < shootFrequency) return;
		timeSinceLastShot = 0;
		var bullet = Instantiate(projectile, spawnLocation.position, spawnLocation.rotation);
		bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.rotation * (Vector3.forward * power), ForceMode.Impulse);
		firedProjectiles.Add(bullet);
		audioSource.PlayOneShot(fireSounds[UnityEngine.Random.Range(0, fireSounds.Length)]);
		if (bullet.scene != gameObject.scene) SceneManager.MoveGameObjectToScene(bullet, gameObject.scene);
	}
}
