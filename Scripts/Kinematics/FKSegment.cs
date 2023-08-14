using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FKSegment : KinematicSegment
{
	[SerializeField] [Range(-90, 90)] float inputAngle;
	[SerializeField] bool enableNoise = false;

	float baseAngle;
	float noise;

	public override void Initialize(KinematicSegment parent, Vector2 position, float angle, float length, float size)
	{
		this.parent = parent;
		this.size = size;
		this.angle = angle;
		this.length = length;

		start = position;
		baseAngle = angle;

		noise = Random.value * 20;
	}

	private void Update()
	{
		// scale segment
		transform.localScale = Vector2.one * size;

		// set segment angle
		float localAngle = inputAngle;

		if (enableNoise)
		{
			noise += Time.deltaTime;
			float t = Mathf.PerlinNoise(noise, 0);
			localAngle += Mathf.Lerp(-90, 90, t);
		}

		// update angle using parent angle
		angle = parent != null ? parent.angle + localAngle : baseAngle + localAngle;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}

