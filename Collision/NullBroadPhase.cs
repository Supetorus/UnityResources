using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullBroadPhase : BroadPhase
{
	public List<Body> bodies;

	public override void Build(AABB aabb, List<Body> bodies)
	{
		queryResultCount = 0;
		this.bodies = bodies;
	}

	public override void Query(AABB aabb, List<Body> results)
	{
		//
	}

	public override void Query(Body body, List<Body> results)
	{
		results.AddRange(bodies);
		queryResultCount += results.Count;
	}

	public override void Draw()
	{
		//
	}
}
