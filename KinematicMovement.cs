using UnityEngine;

public class KinematicMovement : Movement
{
	private void LateUpdate()
	{
		velocity += acceleration * Time.deltaTime;
		float speed = velocity.magnitude;
		speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
		velocity = velocity.normalized * speed;//Vector3.ClampMagnitude(velocity, maxSpeed);
		transform.position += velocity * Time.deltaTime;

		transform.position = Utilities.Wrap(transform.position, new Vector3(-20, -20, -20), new Vector3(20, 20, 20));

		if (movementData.orientToMovement && acceleration.sqrMagnitude > 0.1f)
		{
			transform.rotation = Quaternion.LookRotation(velocity);
		}

		acceleration = Vector3.zero;
	}

	public override void MoveTowards(Vector3 target)
	{
		Vector3 direction = (target - transform.position).normalized;
		ApplyForce(direction * maxForce);
	}

	public override void ApplyForce(Vector3 force)
	{
		acceleration += force;
	}

	public override void Stop()
	{
		velocity = Vector3.zero;
	}

	public override void Resume()
	{
		//
	}

}
