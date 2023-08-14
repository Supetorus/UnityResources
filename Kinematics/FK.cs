using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FK : MonoBehaviour
{
	[SerializeField] FKSegment segmentPrefab;
	[SerializeField] int segmentCount = 5;
	[SerializeField] [Range(0.1f, 3.0f)] float size = 1;
	[SerializeField] [Range(0.1f, 3.0f)] float length = 1;

	List<FKSegment> segments = new List<FKSegment>();

	private void Start()
	{
		KinematicSegment parent = null;

		// create segments
		for (int i = 0; i < segmentCount; i++)
		{
			// create segment
			var segment = Instantiate(segmentPrefab, transform);
			segment.Initialize(parent, transform.position, transform.rotation.eulerAngles.z, length, size);

			// add segment
			segments.Add(segment);

			// set parent to current segment
			parent = segment;
		}
	}

	void Update()
	{
		// update segments
		foreach (var segment in segments)
		{
			// update size and length
			segment.length = length;
			segment.size = size;

			// set segment start to segment parent end
			if (segment.parent != null) segment.start = segment.parent.end;
		}
	}
}
