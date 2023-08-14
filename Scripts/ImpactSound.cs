using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ImpactSound : MonoBehaviour
{
	public float minimumThreshold;
	public AudioClip[] sounds;

	private float sqrThreshold;
	private AudioSource audioSource;

	private void Awake()
	{
		sqrThreshold = Mathf.Pow(minimumThreshold, 2);
		audioSource = GetComponent<AudioSource>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.relativeVelocity.sqrMagnitude > sqrThreshold)
		{
			audioSource.volume = Mathf.Clamp(collision.relativeVelocity.sqrMagnitude / 100, 0, 1);
			audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
		}
	}
}
