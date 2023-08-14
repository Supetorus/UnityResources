using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RollerGameManager : Singleton<RollerGameManager>
{
	enum State
	{
		TITLE,
		PLAYER_START,
		GAME,
		PLAYER_DEAD,
		GAME_OVER,
		WIN
	}

	[SerializeField] GameObject playerPrefab;
	[SerializeField] Transform playerSpawn;
	[SerializeField] Transform failSpawn;
	[SerializeField] Transform successSpawn;
	[SerializeField] GameObject mainCamera;

	[SerializeField] GameObject titleScreen;
	[SerializeField] GameObject gameOverScreen;
	[SerializeField] GameObject winScreen;
	[SerializeField] TMP_Text scoreUI;
	[SerializeField] TMP_Text livesUI;
	[SerializeField] TMP_Text timeUI;
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
	float gameTime = 0;
	GameObject player;

	public int Score
	{
		get { return score; }
		set
		{
			score = value;
			scoreUI.text = score.ToString("D2");
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

	public float GameTime
	{
		get { return gameTime; }
		set
		{
			gameTime = value;
			timeUI.text = "<mspace=mspace=36>" + gameTime.ToString("0.0") + "</mspace>";
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
				mainCamera.SetActive(false);
				Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
				player = GameObject.FindGameObjectWithTag("Player");
				startGameEvent?.Invoke();
				state = State.GAME;
				GameTime = 60;
				break;
			case State.GAME:
				GameTime -= Time.deltaTime;
				if (GameTime <= 0)
				{
					gameTime = 0;
					state = State.GAME_OVER;
					stateTimer = 5;
					if (GameObject.FindGameObjectsWithTag("Coin").Length > 0) player.GetComponent<Health>().Damage(1000000f);
					else
					{
						player.transform.position = successSpawn.position;
						player.GetComponent<Rigidbody>().useGravity = false;
						winScreen.SetActive(true);
					}
					state = State.WIN;
				}
				break;
			case State.PLAYER_DEAD:
				//if (stateTimer <= 0)
				//{
				//	//state = State.PLAYER_START;
				//}
				break;
			case State.GAME_OVER:
				//if (stateTimer <= 0)
				//{
				//	state = State.TITLE;
				//	gameOverScreen.SetActive(false);
				//	titleScreen.SetActive(true);
				//}
				break;
			case State.WIN:

				break;
			default:
				break;
		}
	}

	public void OnStartGame()
	{
		gameTime = 0;
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
		//mainCamera.SetActive(true);
		Instantiate(playerPrefab, failSpawn.position, failSpawn.rotation);
		//player.transform.position = failSpawn.position;
		//Lives --;
		//if (Lives <= 0)
		//{
		state = State.GAME_OVER;
		//	stateTimer = 5;
		gameOverScreen.SetActive(true);
		//}
		//else
		//{
		//	state = State.PLAYER_DEAD;
		//	stateTimer = 3;
		//}
		stopGameEvent?.Invoke();
	}

	private void DestroyAllEnemies()
	{
		// destroy all enemies
		//var spaceEnemies = FindObjectsOfType<SpaceEnemy>();
		//foreach (var spaceEnemy in spaceEnemies)
		//{
		//	Destroy(spaceEnemy.gameObject);
		//}
		//var bullets = GameObject.FindGameObjectsWithTag("Bullet");
		//foreach (var bullet in bullets)
		//{
		//	Destroy(bullet.gameObject);
		//}
	}
}
