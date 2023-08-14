using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Integrator
{
	public static void ExplicitEuler(Body body, float dt)
	{
		body.Position += body.Velocity * dt;
		body.Velocity += body.Acceleration * dt;

		body.Velocity += Force.ApplyDrag(body.Velocity, body.drag) * dt;
	}

	public static void SemiImplicitEuler(Body body, float dt)
	{
		body.Velocity += body.Acceleration * dt;
		body.Position += body.Velocity * dt;

		body.Velocity += Force.ApplyDrag(body.Velocity, body.drag) * dt;
	}
}
