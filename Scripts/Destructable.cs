using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destructable : MonoBehaviour
{
	[Tooltip("The velocity at which something has to hit this in order to destroy it.")]
	public float destroyVelocity;
	public AudioClip[] sounds;

	[HideInInspector] public bool isGhost = false;
	[HideInInspector] public UnityEvent OnDestroy;

	private void OnCollisionEnter(Collision collision)
	{
		if (!isGhost && collision.relativeVelocity.magnitude > destroyVelocity)
		{
			Destroy(gameObject, 0.1f);
			if (sounds.Length > 0) AudioSource.PlayClipAtPoint(sounds[Random.Range(0, sounds.Length)], transform.position);
		}
	}
}
