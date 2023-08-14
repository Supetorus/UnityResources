using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK : MonoBehaviour
{
	[SerializeField] IKSegment prefabSegment;
	[SerializeField] int segmentCount = 5;
	[SerializeField][Range(0.1f, 3.0f)] float startingSize = 1;
	[SerializeField][Range(0.1f, 3.0f)] float maxLength = 1;

	[SerializeField] Transform targetTransform;
	[SerializeField] Transform anchor;

	List<IKSegment> segments = new List<IKSegment>();
	private float currentLength;

	private void Start()
	{
		currentLength = 0;
		KinematicSegment parent = null;

		// create segments
		for (int i = 0; i < segmentCount; i++)
		{
			// create segment
			var segment = Instantiate(prefabSegment, transform);
			segment.Initialize(parent, transform.position, transform.rotation.eulerAngles.z, currentLength, startingSize);
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
			segment.size = startingSize;
			//segment.length = currentLength;
			segment.length = currentLength;

			// set target to parent start if parent exists else set target to target transform position
			Vector2 target = segment.end;

			if (segment.parent != null)
			{
				target = segment.parent.start;
			}
			else if (targetTransform != null)
			{
				target = (Vector2)targetTransform.position;
				targetTransform.transform.position = segment.end;
			}

			// call segment follow with target as parameter
			segment.Follow(target);
		}

		// if anchor is present
		if (anchor != null)
		{
			// start at the base (last segment = segment count - 1) 
			int base_index = segmentCount - 1;
			// set base segment start to anchor position
			segments[base_index].start = anchor.position;
			//iterate from the back to the front, setting segment start to next segment end
			for (int i = base_index - 1; i >= 0; i--)
			{
				segments[i].start = segments[i + 1].end;
			}
			//for (base_index -= 1; base_index >= 0; base_index--)
			//{
			//	segments[base_index].start = segments[base_index + 1].end;
			//}
		}
	}

	//public IEnumerator ReelIn()
	//{
	//	while (currentLength > length * .05f)
	//	{
	//		currentLength = currentLength - (length * .01f);
	//		yield return new WaitForEndOfFrame();
	//	}
	//}

	public void Grab(KinematicTarget target)
	{
		currentLength = maxLength;
		targetTransform = target.transform;
		StartCoroutine(Retract());
	}

	public void Release()
	{
		targetTransform = null;
	}

	private IEnumerator Retract()
	{
		var startLocation = targetTransform.position;
		float num = 1;
		while (Mathf.Abs((targetTransform.position - transform.position).magnitude) > 0.1f)
		{
			//currentLength -= Mathf.Lerp(maxLength, 0, num -= 0.01f);
			targetTransform.position = Vector3.Lerp(transform.position, startLocation, num -= 0.005f);
			yield return new WaitForEndOfFrame();
		}
		Destroy(targetTransform.gameObject);
	}
}
