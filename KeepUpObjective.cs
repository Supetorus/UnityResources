using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepUpObjective : MonoBehaviour
{
	public Transform Level;
	private bool dropped = false;

	private void Awake()
	{
		Game.Instance.RegisterObjective(this);
	}

	private void Update()
	{
		if (!dropped && transform.position.y < Level.position.y)
		{
			Game.Instance.DropObjective(this);
			dropped = true;
		}
	}
}
