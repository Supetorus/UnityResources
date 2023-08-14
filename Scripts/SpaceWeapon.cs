using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceWeapon : MonoBehaviour
{
	[SerializeField] GameObject projectilePrefab;
	[SerializeField] Transform[] spawnTransforms;
	[SerializeField] float fireRate;

	float fireTimer = 0;

	private void Update()
	{
		fireTimer -= Time.deltaTime;
	}

	public void Fire()
	{
		if (fireTimer <= 0)
		{
			fireTimer = fireRate;
			foreach (Transform t in spawnTransforms)
			{
				Instantiate(projectilePrefab, t.position, t.rotation);
			}
		}
	}
}
