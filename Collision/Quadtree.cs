using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quadtree : BroadPhase
{
	public int capacity { get; set; } = 4;
	QuadtreeNode rootNode;

	public override void Build(AABB aabb, List<Body> bodies)
	{
		queryResultCount = 0;
		// create quadtree root node
		rootNode = new QuadtreeNode(aabb, capacity, 0);
		// insert bodies starting at root node
		bodies.ForEach(body => rootNode.Insert(body));
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
