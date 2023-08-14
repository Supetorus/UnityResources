using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathViewer : MonoBehaviour
{
	enum SearchType
	{
		DFS,
		BFS,
		DIJKSTRA,
		ASTAR
	}

	[Range(0, 500)] public int steps = 0;
	[SerializeField] GraphNodeSelector nodeSelector;
	[SerializeField] SearchType searchType;
	[SerializeField] bool visible = true;
	[SerializeField] [TextArea] string info;

	//public bool visible { get; set; } = true;

	int prevSteps;
	bool found = false;
	List<GraphNode> path = new List<GraphNode>();

	Search.SearchAlgorithm[] searchAlgorithms = { Search.DFS, Search.BFS, Search.Dijkstra, Search.AStar, null };
	Search.SearchAlgorithm searchAlgorithm;
	SearchType prevSearchType;

	private void Start()
	{
		prevSteps = steps;
		searchAlgorithm = searchAlgorithms[(int)searchType];
		prevSearchType = searchType;
	}

	private void Update()
	{
		// Update Search Type
		if (searchType != prevSearchType)
		{
			searchAlgorithm = searchAlgorithms[(int)searchType];
			BuildPath();
		}
		prevSearchType = searchType;

		// build path
		steps = Mathf.Clamp(steps, 0, 500);
		if (steps != prevSteps)
		{
			BuildPath();
		}
		prevSteps = steps;

		var nodes = Node.GetNodes<GraphNode>();
		nodes.ToList().ForEach(node => node.GetComponent<Renderer>().enabled = visible);

		if (visible)
		{
			// show node connections
			nodes = Node.GetNodes<GraphNode>();
			nodes.ToList().ForEach(node => node.neighbors.ForEach(neighbor => Debug.DrawLine(node.transform.position, neighbor.transform.position)));

			// reset graph nodes color
			nodes.ToList().ForEach(node => node.GetComponent<Renderer>().material.color = node.visited ? Color.black : Color.white);

			// set all path nodes color
			Color color = (found) ? Color.yellow : Color.magenta;
			path.ForEach(node => node.GetComponent<Renderer>().material.color = color);
		}
	}

	public void BuildPath()
	{
		// reset graph nodes
		GraphNode.ResetNodes();

		// build path
		found = Search.BuildPath(searchAlgorithm, nodeSelector.sourceNode, nodeSelector.destinationNode, ref path, steps);
	}

	public void ShowNodes()
	{
		visible = true;
		Node.ShowNodes<GraphNode>();
	}

	public void HideNodes()
	{
		visible = false;
		Node.HideNodes<GraphNode>();
	}
}
