using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[SerializeField] List<Checkpoint> checkpoints = new List<Checkpoint>();
	[SerializeField] IntData numberOfLives;
	[SerializeField] GameObject playerPrefab;

	private Checkpoint currentCheckpoint;

	private void Awake()
	{
		if (checkpoints.Count > 0) currentCheckpoint = checkpoints[0];
	}

	public void PlayerDead()
	{
		numberOfLives.value -= 1;
	}

	public void Respawn()
	{
		Instantiate(playerPrefab, currentCheckpoint.spawnPoint.position, currentCheckpoint.spawnPoint.rotation);
	}
}
