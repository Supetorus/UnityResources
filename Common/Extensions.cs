using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
	public static Vector2 Wrap(this Vector2 v, Vector2 min, Vector2 max)
	{
		v.x = (v.x > max.x) ? min.x : (v.x < min.x) ? max.x : v.x;
		v.y = (v.y > max.y) ? min.y : (v.y < min.y) ? max.y : v.y;


		return v;
	}
}
