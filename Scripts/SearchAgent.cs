using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchAgent : Agent
{
	[SerializeField] protected Node initialNode;

	public Node targetNode { get; set; } // the current goal node
	public GraphNode sourceNode { get; set; } // the current goal node
	public GraphNode destinationNode { get; set; } // the current goal node

	List<GraphNode> path = new List<GraphNode>();

	private void Start()
	{
		sourceNode = Node.GetRandomNode<GraphNode>();
		destinationNode = Node.GetRandomNode<GraphNode>();
		targetNode = sourceNode;

		GeneratePath();
	}

	// Update is called once per frame
	void Update()
	{
		if (targetNode != null)
		{
			movement.MoveTowards(targetNode.transform.position);
		}
		Debug.DrawLine(transform.position, sourceNode.transform.position, Color.green);
		Debug.DrawLine(transform.position, destinationNode.transform.position, Color.red);
		Debug.DrawLine(transform.position + Vector3.up, targetNode.transform.position + Vector3.up, Color.yellow);
	}

	public GraphNode GetNextNode(GraphNode graphNode)
	{
		if (path.Count == 0) return null;

		int index = path.FindIndex(node => node == graphNode);
		if (index == path.Count - 1)
		{
			sourceNode = destinationNode;
			do
			{
				destinationNode = Node.GetRandomNode<GraphNode>();
			} while (sourceNode == destinationNode);
			GeneratePath();
			index = 0;
		}

		GraphNode nextNode = path[index + 1];

		return nextNode;
	}

	private void GeneratePath()
	{
		GraphNode.ResetNodes();
		Search.BuildPath(Search.AStar, sourceNode, destinationNode, ref path);
	}
}
