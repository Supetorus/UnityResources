using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public abstract class Trajectory : MonoBehaviour
{
	public int steps;
	public float interval;
	protected LineRenderer lineRenderer;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	public abstract void CalculateTrajectory(Vector3 startPosition, Vector3 velocity);
}
