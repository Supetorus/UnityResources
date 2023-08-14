using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AABB
{
	public Vector2 center;
	public Vector2 size;
	public Vector2 extents { get { return size * 0.5f; } }

	public Vector2 min { get => center - extents; set => SetMinMax(value, max); } // Top left corner
	public Vector2 max { get => center + extents; set => SetMinMax(max, value); } // Bottom right corner

	public AABB(Vector2 center, Vector2 size)
	{
		this.center = center;
		this.size = size;
	}

	public bool Contains(AABB other)
	{
		return	(min.x <= other.max.x && max.x >= other.min.x) &&
				(min.y <= other.max.y && max.y >= other.min.y);
	}

	public bool Contains(Vector2 point)
	{
		return	(point.x >= min.x && point.x <= max.x) &&
				(point.y >= min.y && point.y <= max.y);
	}

	public void SetMinMax(Vector2 min, Vector2 max)
	{
		size = (max - min);
		center = min + extents;
	}

	public void Expand(Vector2 point)
	{
		SetMinMax(Vector2.Min(min, point), Vector2.Max(max, point));
	}

	public void Expand(AABB aabb)
	{
		SetMinMax(Vector2.Min(min, aabb.min), Vector2.Max(max, aabb.max));
	}

	public void Draw(Color color, float width = 0.05f)
	{
		Debug.DrawLine(new Vector2(min.x, min.y), new Vector2(max.x, min.y), color);
		Debug.DrawLine(new Vector2(min.x, max.y), new Vector2(max.x, max.y), color);
		Debug.DrawLine(new Vector2(min.x, min.y), new Vector2(min.x, max.y), color);
		Debug.DrawLine(new Vector2(max.x, min.y), new Vector2(max.x, max.y), color);
	}
}