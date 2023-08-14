using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring
{
	public Body BodyA { get; set; }
	public Body BodyB { get; set; }
	public float RestLength { get; set; }
	public float K { get; set; }

	public void ApplyForce()
	{
		Vector2 direction = BodyA.Position - BodyB.Position;
		float length = direction.magnitude;
		float x = length - RestLength;

		float f = -K * x;

		BodyA.ApplyForce( f * direction.normalized, Body.ForceMode.FORCE);
		BodyB.ApplyForce(-f * direction.normalized, Body.ForceMode.FORCE);

		Debug.DrawLine(BodyA.Position, BodyB.Position);
	}

	public static Vector2 Force(Vector2 positionA, Vector2 positionB, float restLength, float k)
	{
		Vector2 direction = positionA - positionB;
		float length = direction.magnitude;
		float x = length - restLength;

		float f = -k * x;
		return f * direction.normalized;
	}
}
