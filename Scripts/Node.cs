using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
	[SerializeField, Tooltip("Each location where a new block could be spawned, rotated in the direction of travel.")] Transform[] edgePositions;
	[SerializeField, Tooltip("All of the prefab blocks which could be spawned at any of the edges.")] PrefabReference[] possibleTransitions;

	public Node parent;
	public List<Node> children = new List<Node>();
	/// <summary>
	/// When a node is triggered it is marked as selected so it isn't pruned and the ground doesn't drop out from underneath the player.
	/// </summary>
	public bool IsSelectedPath = false;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag != "Player") return;
		Generate(2);
		IsSelectedPath = true;
		parent?.Prune();
		if (parent != null) Destroy(parent, 1);
	}

	private void Generate(int depth)
	{
		depth -= 1;
		if (depth < 0) return;

		if (children.Count == 0)
		{
			foreach (var edge in edgePositions)
			{
				Node childNode = Instantiate(possibleTransitions[Random.Range(0, possibleTransitions.Length)].prefab,
					edge.transform.position,
					edge.transform.rotation)
					.GetComponent<Node>();
				children.Add(childNode);
				childNode.parent = this;
			}
		}
		foreach (Node child in children) child.Generate(depth);
	}

	public void Prune()
	{
		foreach (Node n in children)
		{
			if (!n.IsSelectedPath) n.DestroyBranch();
		}
		if (gameObject != null) Destroy(gameObject, 2);
	}

	private void DestroyBranch()
	{
		foreach (Node child in children)
		{
			child.DestroyBranch();
		}
		if (!IsSelectedPath && gameObject != null) Destroy(gameObject);
	}
}
