using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
	public Path path;
	public string pathName;
	public Node targetNode { get; set; }
	public bool complete { get => targetNode == null; }

	public void Move(Movement movement)
	{
		if (targetNode != null)
		{
			movement.MoveTowards(targetNode.transform.position);
		}
	}

	static Path GetPathByName(string name)
	{
		var paths = GameObject.FindObjectsOfType<Path>();
		foreach (var path in paths)
		{
			if (path.name.ToLower() == name.ToLower())
			{
				return path;
			}
		}

		return null;
	}

	public static Path GetRandomPath()
	{
		var paths = GameObject.FindObjectsOfType<Path>();

		return paths[Random.Range(0, paths.Length)];
	}

	public void Start()
	{
		if (path == null)
		{
			path = (pathName.Length != 0) ? GetPathByName(pathName) : GetRandomPath();
		}
	}

}
