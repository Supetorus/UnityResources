using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle
{
	public Vector2 center;
	public float radius;

	public Circle(Vector2 center, float radius)
	{
		this.center = center;
		this.radius = radius;
	}

	public Circle(Body body)
	{
		center = body.Position;
		radius = ((CircleShape)body.shape).Radius;

	}

	public static bool Intersects(Circle circleA, Circle circleB)
	{
		float distance = (circleA.center - circleB.center).magnitude;
		float combinedRadius = circleA.radius + circleB.radius;
		return distance <= combinedRadius;
	}
}
