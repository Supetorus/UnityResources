using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Locked : MonoBehaviour
{
	[SerializeField] private Key key;
	public UnityEvent onUnlock;

	private void Start()
	{
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Inventory>().HasKey(key))
		{
			onUnlock?.Invoke();
		}
	}

	public void FreeMovement()
	{
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
	}
}
