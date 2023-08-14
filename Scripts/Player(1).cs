using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	//Range sets the range that speed can be set to in the inspector.
	//Tooltip creates a tooltip in the inspector.
	[Range(0, 10)] [Tooltip("speed of the player")]public float speed = 5;

	[SerializeField] AudioSource audioSource;

	// Update is called once per frame
	void Update()
	{
		Vector3 direction = Vector3.zero;

		direction.x = Input.GetAxis("Horizontal");
		direction.z = Input.GetAxis("Vertical");

		transform.position += direction * speed * Time.deltaTime;
		//transform.rotation *= Quaternion.Euler(1, 0, 0);
		//transform.localScale = new Vector3(2, 2, 2);

		if (Input.GetButton("Fire1"))
		{
			//GetComponent<AudioSource>().Play();
			audioSource.Play();
			//GetComponent<Renderer>().material.color = Color.green;
			transform.rotation *= Quaternion.Euler(0, 0, 1);
		}

		//GameObject go = GameObject.Find("Cube");
		//if (go != null) go.GetComponent<Renderer>().material.color = Color.cyan;
	}
}
