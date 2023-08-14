using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Row : MonoBehaviour
{
	public List<GameObject> images = new List<GameObject>();

	public bool Remove()
	{
		Destroy(images[images.Count - 1]);
		images.RemoveAt(images.Count - 1);

		return images.Count == 0;
	}
}
