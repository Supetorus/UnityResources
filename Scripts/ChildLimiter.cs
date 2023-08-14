using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildLimiter : MonoBehaviour
{
	[SerializeField] List<GameObject> children;

	public void Limit(int limit)
	{
		for (int i = 0; i < children.Count; i++)
		{
			children[i].SetActive(i <= limit);
		}
	}
}
