using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
	[SerializeField] PrefabReference respawnPrefab;
	[SerializeField] IntData lives;

    public Checkpoint CurrentCheckpoint { get; set; }

    public void Respawn()
	{
		if (CurrentCheckpoint != null && lives.value > 0)
		{
			Instantiate(respawnPrefab.prefab, CurrentCheckpoint.spawnPoint.position, CurrentCheckpoint.spawnPoint.rotation);
		}
	}
}
