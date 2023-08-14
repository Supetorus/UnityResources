using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjective : MonoBehaviour
{
	public Transform level;
	private bool dropped = false;

	private void Awake()
	{
		Game.Instance.RegisterObjective(this);
	}

	private void Update()
	{
		if (!dropped && transform.position.y < level.position.y)
		{
			Game.Instance.DropObjective(this);
			dropped = true;
		}
	}

}
