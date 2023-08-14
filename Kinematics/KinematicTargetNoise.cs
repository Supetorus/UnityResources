using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicTargetNoise : MonoBehaviour
{
	[SerializeField][Range(0.1f, 10.0f)] float length = 1;
	[SerializeField][Range(0, 360)] float angle = 0;
	[SerializeField][Range(0, 10)] float range = 1;

	[SerializeField][Range(0, 2)] float noiseRate = 0;
	[SerializeField] bool enableNoise = false;
	[SerializeField] bool enableRaycast = false;

	float noise;

	void Start()
	{
		noise = Random.value * 100;
	}

	void Update()
	{
		// noise
		Vector2 noiseOffset = Vector2.zero;
		if (enableNoise)
		{
			noise += Time.deltaTime * noiseRate;
			//float t1 = Random.value;
			//float t2 = Random.value;

			float t1 = Mathf.PerlinNoise(noise, 0);
			float t2 = Mathf.PerlinNoise(0, noise);


			noiseOffset.x = Mathf.Lerp(-range, range, t1);
			noiseOffset.y = Mathf.Lerp(-range, range, t2);
		}


		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		Vector3 localPosition = rotation * (Vector3.right * length) + (Vector3)noiseOffset;
		Vector3 position = transform.parent.position + localPosition;
		//Vector3 position = localPosition;

		Debug.DrawLine(transform.parent.position, position);

		if (enableRaycast)
		{
			Vector3 direction = position - transform.parent.position;
			var hit = Physics2D.Raycast(transform.parent.position, direction, direction.magnitude);
			if (hit)
			{
				position = hit.point;
			}
		}

		// update position
		transform.position = position;
	}
}
