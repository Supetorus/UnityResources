using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : Singleton<GameManager>
{
	enum State
	{
		TITLE,
		PLAYER_START,
		GAME,
		PLAYER_DEAD,
		GAME_OVER
	}

	[SerializeField] GameObject playerPrefab;
	[SerializeField] Transform playerSpawn;
	[SerializeField] BoxSpawner boxSpawner;

	[SerializeField] GameObject titleScreen;
	[SerializeField] GameObject gameOverScreen;
	[SerializeField] TMP_Text scoreUI;
	[SerializeField] TMP_Text livesUI;
	[SerializeField] Slider healthBarUI;
	[SerializeField] int startingLives = 3;

	public float playerHealth { set { healthBarUI.value = value; } }

	public delegate void GameEvent();

	public event GameEvent startGameEvent;
	public event GameEvent stopGameEvent;

	int score = 0;
	int lives;
	State state = State.TITLE;
	float stateTimer;
	float gameTimer = 0;

	public int Score
	{
		get { return score; }
		set
		{
			score = value;
			scoreUI.text = score.ToString();
		}
	}

	public int Lives
	{
		get { return lives; }
		set
		{
			lives = value;
			livesUI.text = "Lives: " + lives.ToString();
		}
	}

	private void Update()
	{
		stateTimer -= Time.deltaTime;
		switch (state)
		{
			case State.TITLE:
				break;
			case State.PLAYER_START:
				DestroyAllEnemies();
				Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
				startGameEvent?.Invoke();
				state = State.GAME;
				boxSpawner.timeModifier = 1;
				break;
			case State.GAME:
				gameTimer += Time.deltaTime;
				if (gameTimer > 5)
				{
					gameTimer = 0;
					boxSpawner.timeModifier -= 0.1f;
					boxSpawner.timeModifier = Mathf.Max(0.2f, boxSpawner.timeModifier);
				}
				break;
			case State.PLAYER_DEAD:
				if (stateTimer <= 0)
				{
					state = State.PLAYER_START;
				}
				break;
			case State.GAME_OVER:
				if (stateTimer <= 0)
				{
					state = State.TITLE;
					gameOverScreen.SetActive(false);
					titleScreen.SetActive(true);
				}
				break;
			default:
				break;
		}
	}

	public void OnStartGame()
	{
		gameTimer = 0;
		Lives = startingLives;
		state = State.PLAYER_START;
		titleScreen.SetActive(false);
	}

	public void OnStartTitle()
	{
		state = State.TITLE;
		Score = 0;
		Lives = 3;
		titleScreen.SetActive(true);
		stopGameEvent?.Invoke();
	}

	public void OnPlayerDead()
	{
		Lives -= 1;
		if (Lives <= 0)
		{
			state = State.GAME_OVER;
			stateTimer = 5;
			gameOverScreen.SetActive(true);
		}
		else
		{
			state = State.PLAYER_DEAD;
			stateTimer = 3;
		}
		stopGameEvent?.Invoke();
	}

	private void DestroyAllEnemies()
	{
		// destroy all enemies
		var spaceEnemies = FindObjectsOfType<SpaceEnemy>();
		foreach (var spaceEnemy in spaceEnemies)
		{
			Destroy(spaceEnemy.gameObject);
		}
		var bullets = GameObject.FindGameObjectsWithTag("Bullet");
		foreach (var bullet in bullets)
		{
			Destroy(bullet.gameObject);
		}

	}
}
