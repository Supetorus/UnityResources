using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePerception : MonoBehaviour
{
	[SerializeField] [Range(1, 20)] float distance;
	[SerializeField] [Range(0, 10)] float radius;
	[SerializeField] [Range(2, 30)] public int numRaycast = 2;
	[SerializeField] LayerMask layerMask;

	public bool IsObstacleInFront()
	{
		Ray ray = new Ray(transform.position, transform.forward);
		return Physics.SphereCast(ray, radius, distance, layerMask);
	}

	public Vector3 GetOpenDirection()
	{
		Vector3[] directions = GetDirectionsInCircle();
		foreach (Vector3 direction in directions)
		{
			Ray ray = new Ray(transform.position, transform.TransformDirection(direction));
			if (!Physics.SphereCast(ray, radius, distance, layerMask))
			{
				//Debug.DrawRay(ray.origin, ray.direction * distance, Color.white);
				return ray.direction;
			}
		}
		return transform.forward;
	}

	Vector3[] GetDirectionsInCircle()
	{
		List<Vector3> result = new List<Vector3>();

		float angleOffset = 180 / (numRaycast - 1);
		for (int i = 0; i < numRaycast; i++)
		{
			result.Add(Quaternion.AngleAxis(angleOffset * i, Vector3.up) * Vector3.forward);
			result.Add(Quaternion.AngleAxis(-angleOffset * i, Vector3.up) * Vector3.forward);
		}

		return result.ToArray();
	}
}
