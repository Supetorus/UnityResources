using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RollerPlayer : MonoBehaviour, IDestructable
{
	[SerializeField] float maxForce = 5;
	[SerializeField] float jumpForce = 5;
	[SerializeField] ForceMode forceMode;
	[SerializeField] Transform viewTransform;

	Rigidbody rb;
	Vector3 force = Vector3.zero;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}


	// Update is called once per frame
	void Update()
	{
		// Movement
		Vector3 direction = Vector3.zero;
		direction.x = Input.GetAxis("Horizontal");
		direction.z = Input.GetAxis("Vertical");

		// 
		Quaternion viewSpace = Quaternion.AngleAxis(viewTransform.rotation.eulerAngles.y, Vector3.up);
		force = (viewSpace * direction) * maxForce;

		if (Input.GetButtonDown("Jump") && isGrounded())
		{
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}

		RollerGameManager.Instance.playerHealth = GetComponent<Health>().health;
	}

	private void FixedUpdate()
	{
		rb.AddForce(force, forceMode);
	}

	public void Destroyed()
	{
		RollerGameManager.Instance.OnPlayerDead();
	}
	
	private bool isGrounded()
	{
		return Physics.Raycast(transform.position, Vector3.down, 0.55f)
			|| Physics.Raycast(transform.position, new Vector3(0.2f, -0.55f, 0), 0.55f)
			|| Physics.Raycast(transform.position, new Vector3(0.2f, -0.55f, 0.2f), 0.55f)
			|| Physics.Raycast(transform.position, new Vector3(0.2f, -0.55f, -0.2f), 0.55f)
			|| Physics.Raycast(transform.position, new Vector3(0, -0.55f, 0.2f), 0.55f)
			|| Physics.Raycast(transform.position, new Vector3(0, -0.55f, -0.2f), 0.55f)
			|| Physics.Raycast(transform.position, new Vector3(-0.2f, -0.55f, 0), 0.55f)
			|| Physics.Raycast(transform.position, new Vector3(-0.2f, -0.55f, 0.2f), 0.55f)
			|| Physics.Raycast(transform.position, new Vector3(-0.2f, -0.55f, -0.2f), 0.55f);
	}
}
