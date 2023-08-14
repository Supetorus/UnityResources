using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BroadPhase
{
	public abstract void Build(AABB aabb, List<Body> bodies);
	public abstract void Query(AABB aabb, List<Body> results);
	public abstract void Query(Body body, List<Body> results);
	public abstract void Draw();

	public int queryResultCount;
	public static readonly Color[] colors = { Color.white, Color.red, Color.green, Color.blue, Color.yellow };
}
