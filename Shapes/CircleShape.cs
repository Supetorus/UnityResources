using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShape : Shape
{
	public override float Size { get => transform.localScale.x; set => transform.localScale = Vector2.one * value; }
	public override float Area => Mathf.Pow(Radius, 2) * Mathf.PI;
	public float Radius { get => Size * 0.5f; }

	public override AABB GetAABB(Vector2 position)
	{
		return new AABB(position, Vector2.one * Size);
	}
}
