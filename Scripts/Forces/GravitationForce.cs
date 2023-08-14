using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationForce : Force
{

	[SerializeField] FloatData gravitation;

	public override void ApplyForce(List<Body> bodies)
	{
		if (gravitation.value == 0) return;

		for (int i = 0; i < bodies.Count - 1; i++)
		{
			for (int j = i + 1; j < bodies.Count; j++)
			{
				Body bodyA = bodies[i];
				Body bodyB = bodies[j];
				Vector2 direction = bodyA.Position - bodyB.Position;
				float distance = Mathf.Max(direction.magnitude, 1);
				float gravityForce = gravitation.value * ((bodyA.Mass * bodyB.Mass) / distance);
				bodyA.ApplyForce(-direction.normalized * gravityForce, Body.ForceMode.FORCE);
				bodyB.ApplyForce(direction.normalized * gravityForce, Body.ForceMode.FORCE);
			}
		}
	}
}
