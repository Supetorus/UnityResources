using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroll : MonoBehaviour
{
	[SerializeField] Vector2 scrollRate;
	MeshRenderer meshRenderer;

	private void Awake()
	{
		meshRenderer = GetComponent<MeshRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		Vector2 offset = Time.time * scrollRate;
		meshRenderer.material.SetTextureOffset("_MainTex", offset);
	}
}
