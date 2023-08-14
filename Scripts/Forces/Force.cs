using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Force : MonoBehaviour
{
	public abstract void ApplyForce(List<Body> bodies);

	public static Vector2 ApplyDrag(Vector2 v, float drag)
	{
		return (0.5f * v.sqrMagnitude * drag) * -v.normalized;
	}
}
