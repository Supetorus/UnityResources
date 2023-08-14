using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] GameObject prefab;

	private void Start()
	{
		//Destroy(gameObject, 6);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			GameObject go = Instantiate(prefab, transform.position, Quaternion.identity);
			Destroy(go, 4);
		}
	}
}
