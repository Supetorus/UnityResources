using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
	public enum ForceMode
	{
		FORCE,
		ACCELERATION,
		VELOCITY
	}

	public enum eBodyType
	{
		STATIC,
		KINEMATIC,
		DYNAMIC,
	}

	public Shape shape;
	[HideInInspector] public List<Spring> springs = new List<Spring>();

	public float drag { get; set; } = 0;
	public float restitution { get; set; } = 1.0f;
	public eBodyType BodyType { get; set; } = eBodyType.DYNAMIC;
	public Vector2 Position { get => transform.position; set => transform.position = value; }
	public Vector2 Velocity { get; set; } = Vector2.zero;
	public Vector2 Acceleration { get; set; } = Vector2.zero;
	public Vector2 Force { get; set; } = Vector2.zero;
	public float Mass => shape.Mass;
	public float InverseMass { get => Mass == 0 || BodyType != eBodyType.DYNAMIC ? 0 : 1 / Mass; }

	public void ApplyForce(Vector2 force, ForceMode forceType)
	{
		if (BodyType != eBodyType.DYNAMIC) return;

		switch (forceType)
		{
			case ForceMode.FORCE:
				Acceleration += force * InverseMass;
				break;
			case ForceMode.ACCELERATION:
				Acceleration += force;
				break;
			case ForceMode.VELOCITY:
				Velocity = force;
				break;
			default:
				break;
		}

	}

	public void Step(float dt)
	{
		//Acceleration = Simulator.Instance.gravity + (Force * InverseMass);
		Acceleration = Force * InverseMass;// * dt;
	}
}
