using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHNode
{
	AABB nodeAABB;

	List<Body> nodeBodies;

	BVHNode left;
	BVHNode right;

	public BVHNode(List<Body> bodies)
	{
		nodeBodies = bodies;
		ComputeBoundary();
		Split();
	}

	public void ComputeBoundary()
	{
		if (nodeBodies.Count > 0) 
		{
			nodeAABB.center = nodeBodies[0].Position;
			nodeAABB.size = Vector3.zero;

			nodeBodies.ForEach(body => this.nodeAABB.Expand(body.shape.GetAABB(body.Position)));
		}
	}

	public void Split()
	{
		int length = nodeBodies.Count;
		int half = length >> 1;
		if (half >= 1)
		{
			left = new BVHNode(nodeBodies.GetRange(0, half));
			right = new BVHNode(nodeBodies.GetRange(half, half));

			nodeBodies.Clear();
		}
	}

	public void Query(AABB aabb, List<Body> results)
	{
		// Check if query aabb intersects this node's aabb.
		if (!nodeAABB.Contains(aabb)) return;

		// Add intersecting node bodies.
		results.AddRange(nodeBodies);

		left?.Query(aabb, results);
		right?.Query(aabb, results);
	}

	public void Draw()
	{
		nodeAABB.Draw(Color.magenta);
		left?.Draw();
		right?.Draw();
	}
}
