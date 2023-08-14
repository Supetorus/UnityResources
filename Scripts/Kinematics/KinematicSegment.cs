using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KinematicSegment : MonoBehaviour
{
	public Vector2 start { get => transform.position; set => transform.position = value; }
	public Vector2 end { get => start + Polar.PolarToCartesian(polar); }
	public float length { get => polar.length; set => polar.length = value; }
	public float angle { get => polar.angle; set => polar.angle = value; }
	public float size { get; set; }
	public KinematicSegment parent { get; set; }

	protected Polar polar;

	public abstract void Initialize(KinematicSegment parent, Vector2 position, float angle, float length, float width);
}
