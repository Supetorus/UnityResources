using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	[SerializeField] Sprite uncheckedSprite;
	[SerializeField] Sprite checkedSprite;
	/// <summary>
	/// The previous checkpoint, used if the level has linear progression.
	/// </summary>
	[SerializeField] Checkpoint lastCheckpoint;

	public Transform spawnPoint;

	private SpriteRenderer spriteRenderer;
	private AudioSource audioSource;
	//private bool IsChecked = false;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = uncheckedSprite;
		audioSource = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent(out CheckpointManager checkpointManager))
		{
			spriteRenderer.sprite = checkedSprite;
			audioSource.Play();
			//IsChecked = true;
			if (lastCheckpoint != null) lastCheckpoint.Check();
			checkpointManager.CurrentCheckpoint = this;

		}
	}

	public void Check()
	{
		//IsChecked = true;
		spriteRenderer.sprite = checkedSprite;
	}
}
