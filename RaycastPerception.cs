using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPerception : Perception
{
	[SerializeField] Transform raycastTransform;
	[SerializeField] [Range(2, 50)] public int numRaycast = 2;

	public override GameObject[] GetGameObjects()
	{
		List<GameObject> result = new List<GameObject>();

		float angleOffset = (angle * 2) / (numRaycast - 1);
		for (int i = 0; i < numRaycast; i++)
		{
			Quaternion rotation = Quaternion.AngleAxis(-angle + (angleOffset * i), Vector3.up);
			Vector3 direction = rotation * raycastTransform.forward;
			Ray ray = new Ray(raycastTransform.position, direction);
			if (Physics.Raycast(ray, out RaycastHit raycastHit, distance))
			{
				if (tagName == "" || raycastHit.collider.CompareTag(tagName))
				{
					//Debug.DrawRay(ray.origin, ray.direction * raycastHit.distance, Color.red);
					result.Add(raycastHit.collider.gameObject);
				}
			}
			else
			{
				//Debug.DrawRay(ray.origin, ray.direction * distance, Color.white);
			}
		}

		return result.ToArray();
	}
}
