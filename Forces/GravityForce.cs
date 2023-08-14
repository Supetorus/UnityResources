using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityForce : Force
{
	[SerializeField] FloatData gravity;

	public override void ApplyForce(List<Body> bodies)
	{
		foreach(Body b in bodies)
		{
			b.ApplyForce(Vector2.up * gravity.value, Body.ForceMode.ACCELERATION);
		}
	}
}
