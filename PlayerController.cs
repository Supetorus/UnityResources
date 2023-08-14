using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TurnPlayer;

public class PlayerController : MonoBehaviour
{
	[SerializeField, Tooltip("The speed the character moves forward (local z axis).")] float runSpeed = 10;
	[SerializeField, Tooltip("The max speed the player will achieve.")] readonly float maxSpeed = 25;
	[SerializeField, Tooltip("The amount the player accelerates by each second.")] readonly float runAcceleration = 0.01f;
	[SerializeField, Tooltip("How quickly the player moves laterally")] float lateralSpeed = 1;
	[SerializeField, Tooltip("The amount of force that is applied to the character when they jump.")] float jumpForce = 10;
	[SerializeField, Tooltip("The acceleration gravity applies to the player.")] float gravity = -9.8f;
	[SerializeField, Tooltip("The amount of force that is applied over time when the player jumps.")] AnimationCurve jumpForceOverTime;
	[SerializeField, Tooltip("The transform the camera references")] Transform viewTarget;
	[SerializeField, Tooltip("How the viewTarget is offset from the player's transform")] Vector3 viewOffset;
	[SerializeField, Tooltip("The sound played when the player's foot touches the ground.")] AudioSource footSound;

	CharacterController characterController;
	Animator animator;

	/// <summary>
	/// The amount the player's transform will change this frame, assuming they don't collide with anything.
	/// </summary>
	Vector3 currentMovement = Vector3.zero;
	/// <summary>
	/// Which way the player will turn when they reach a turn trigger.
	/// </summary>
	private TurnDirection direction;
	/// <summary>
	/// The approximate distance the player will move when they move laterally.
	/// </summary>
	private float lateralDistance = 1;
	/// <summary>
	/// Represents the lateral position of the player. Can be set to -1, 0, or 1.
	/// </summary>
	private int xPos = 0;
	/// <summary>
	/// Used to keep track of the lateral position of the player in the current "track".
	/// </summary>
	private float localX = 0;
	/// <summary>
	/// The lateral position the player is heading toward.
	/// </summary>
	private float targetXPos;
	/// <summary>
	/// How long before the player touches the turn trigger they can press a turn key, in seconds.
	/// </summary>
	private float turnLeniencyTime;
	/// <summary>
	/// How long the player must wait before pressing a turn key again after a turn has occurred.
	/// </summary>
	private float turnTimer;
	/// <summary>
	/// The amount of time since the player has jumped. Used to calculate jumpForceOverTime.
	/// </summary>
	private float jumpTime = 1;
	private float xStart;

	void Awake()
	{
		characterController = GetComponent<CharacterController>();
		animator = GetComponentInChildren<Animator>();
	}

	private void Start()
	{
		viewTarget.position = transform.position + viewOffset;
	}

	void Update()
	{
		HandleRotation();
		HandleMovement();
		HandleCameraTarget();

		float newSpeed = runSpeed + runAcceleration * Time.deltaTime;

		runSpeed = Mathf.Min(newSpeed, maxSpeed);

		if (transform.position.y <= -5 || characterController.velocity.magnitude == 0)
		{
			Destroyed();
		}
	}

	private void HandleCameraTarget()
	{
		Vector3 playerPosition = viewOffset;
		playerPosition.x = transform.position.x + viewOffset.x;
		playerPosition.z = transform.position.z + viewOffset.z;
		viewTarget.position = playerPosition;
		viewTarget.rotation = Quaternion.Lerp(viewTarget.rotation, transform.rotation, Time.deltaTime * 5);
	}

	private void HandleMovement()
	{
		//Jumping
		currentMovement.y = 0;
		if (Input.GetKey(KeyCode.Space) && characterController.isGrounded)
		{
			jumpTime = 0;
			animator.SetTrigger("Jump");
		}
		else jumpTime = Mathf.Min(jumpTime + Time.deltaTime, 1);
		float currentJumpForce;
		currentJumpForce = Mathf.Clamp(jumpForceOverTime.Evaluate(jumpTime), 0, 1) * jumpForce;

		//Lateral Movement
		if (Input.GetKeyDown(KeyCode.RightArrow) && xPos != 1)
		{
			targetXPos = targetXPos + lateralDistance;
			xStart = localX;
			xPos++;
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow) && xPos != -1)
		{
			targetXPos = targetXPos - lateralDistance;
			xStart = localX;
			xPos--;
		}

		currentMovement.y += gravity;
		currentMovement.y += currentJumpForce;
		currentMovement.z = runSpeed;
		float difference = Mathf.Abs(localX - targetXPos);
		if (localX > targetXPos && difference > 0.05f)
		{
			currentMovement.x = -lateralSpeed;
		}
		else if (localX < targetXPos && difference > 0.05f) currentMovement.x = lateralSpeed;
		else
		{
			currentMovement.x = (targetXPos - localX) * (1 / Time.deltaTime);
		}

		//print("localX " + localX + " targetXPos " + targetXPos + " currentmovement.x " + currentMovement.x);
		localX += currentMovement.x * Time.deltaTime;
		characterController.Move((transform.rotation * currentMovement) * Time.deltaTime);
	}

	private void HandleRotation()
	{
		if (Input.GetKeyDown(KeyCode.A) && turnTimer < 0)
		{
			direction = TurnDirection.LEFT;
			turnLeniencyTime = 1;
			//Turn();
		}
		else if (Input.GetKeyDown(KeyCode.D) && turnTimer < 0)
		{
			direction = TurnDirection.RIGHT;
			turnLeniencyTime = 1;
			//Turn();
		}

		if (turnLeniencyTime <= 0) direction = TurnDirection.NONE;
		turnLeniencyTime -= Time.deltaTime;
		turnTimer -= Time.deltaTime;
	}

	public void Turn(TurnDirection[] directions, Vector3 newPosition)
	{
		foreach (TurnDirection dir in directions)
		{
			if (dir == direction)
			{
				switch (direction)
				{
					case TurnDirection.LEFT:
						transform.Rotate(Vector3.up, -90);
						break;
					case TurnDirection.RIGHT:
						transform.Rotate(Vector3.up, 90);
						break;
				}
				newPosition.y = transform.position.y;
				transform.position = newPosition;
				direction = TurnDirection.NONE;
				turnTimer = 0.5f;
				break;
			}
		}
	}

	public void Destroyed()
	{
		Destroy(gameObject);
		Game.Instance.OnPlayerDead();
	}

	public void FootDown()
	{
		footSound.Play();
	}

	public enum TurnDirection
	{
		LEFT,
		RIGHT,
		NONE
	}

	private enum LateralPosition
	{
		LEFT,
		CENTER,
		RIGHT
	}
}
