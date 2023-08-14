using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVH : BroadPhase
{
	BVHNode rootNode;

	public override void Build(AABB aabb, List<Body> bodies)
	{
		queryResultCount = 0;
		List<Body> sorted = new List<Body>(bodies);

		sorted.Sort((a, b) => a.Position.x.CompareTo(b.Position.x));

		// create BVH root node
		rootNode = new BVHNode(sorted);
	}

	public override void Draw()
	{
		rootNode?.Draw();
	}

	public override void Query(AABB aabb, List<Body> results)
	{
		rootNode.Query(aabb, results);
		queryResultCount += results.Count;
	}

	public override void Query(Body body, List<Body> results)
	{
		rootNode.Query(body.shape.GetAABB(body.Position), results);
	}
}
