using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : Pickup
{

	[SerializeField] GameObject destroyPrefab;
	public int scoreIncrease = 1;

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag != "Player") return;
		Game.Instance.AddMultiplier(scoreIncrease);
		if (destroyPrefab != null) Instantiate(destroyPrefab, transform.position, transform.rotation);
		Destroy(transform.parent.gameObject);
	}
}
