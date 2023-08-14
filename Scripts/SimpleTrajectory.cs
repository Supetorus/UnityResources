using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrajectory: Trajectory
{
public override void CalculateTrajectory(Vector3 startPosition, Vector3 velocity)
{
    //print("Start: " + startPosition + " velocity " + velocity);
	lineRenderer.positionCount = steps;
	for (int i = 0; i < steps; i++)
	{
		lineRenderer.SetPosition(i, PointAtTime(i * interval, startPosition, velocity));
	}
}

private Vector3 PointAtTime(float t, Vector3 startPosition, Vector3 velocity)
{
	//print("Start: " + startPosition + " velocity: " + velocity + " Time: " + t);
	return startPosition + (velocity * t) + 0.5f * Physics.gravity * (t * t);
}
}
