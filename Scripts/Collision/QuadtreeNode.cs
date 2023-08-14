using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeNode
{
	AABB nodeAABB;
	int nodeCapacity;
	int nodeLevel;
	List<Body> nodeBodies = new List<Body>();

	QuadtreeNode northEast;
	QuadtreeNode northWest;
	QuadtreeNode southEast;
	QuadtreeNode southWest;

	bool subdivided = false;

	public QuadtreeNode(AABB aabb, int capacity, int level)
	{
		nodeLevel = level;
		nodeAABB = aabb;
		nodeCapacity = capacity;
	}

	public void Draw()
	{
		Color color = BroadPhase.colors[nodeLevel % BroadPhase.colors.Length];

		nodeAABB.Draw(color);
		nodeBodies.ForEach(body => Debug.DrawLine(nodeAABB.center, body.Position, color));

		northEast?.Draw();
		northWest?.Draw();
		southEast?.Draw();
		southWest?.Draw();
	}

	public void Insert(Body body)
	{
		// Check if intersects
		if (!nodeAABB.Contains(body.Position)) return;

		// Check if under capacity
		if (nodeBodies.Count < nodeCapacity) nodeBodies.Add(body);

		// Subdivide
		else
		{
			// exceeded capacity, subdivide node
			if (!subdivided) Subdivide();

			// insert body into the newly subdivided nodes
			northEast.Insert(body);
			northWest.Insert(body);
			southEast.Insert(body);
			southWest.Insert(body);
		}
	}

	public void Query(AABB aabb, List<Body> results)
	{
		// Check if query aabb intersects this node's aabb.
		if (!nodeAABB.Contains(aabb)) return;

		// Add intersecting node bodies.
		results.AddRange(nodeBodies);

		if (subdivided)
		{ // Check the children
			northEast.Query(aabb, results);
			northWest.Query(aabb, results);
			southEast.Query(aabb, results);
			southWest.Query(aabb, results);
		}
	}

	private void Subdivide()
	{
		float xo = nodeAABB.extents.x * 0.5f;
		float yo = nodeAABB.extents.y * 0.5f;

		northEast = new QuadtreeNode(new AABB(new Vector2(nodeAABB.center.x - xo, nodeAABB.center.y + yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);
		northWest = new QuadtreeNode(new AABB(new Vector2(nodeAABB.center.x + xo, nodeAABB.center.y + yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);
		southEast = new QuadtreeNode(new AABB(new Vector2(nodeAABB.center.x - xo, nodeAABB.center.y - yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);
		southWest = new QuadtreeNode(new AABB(new Vector2(nodeAABB.center.x + xo, nodeAABB.center.y - yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);

		subdivided = true;
	}
}
