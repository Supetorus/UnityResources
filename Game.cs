using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : Singleton<Game>
{
	public List<DropObjective> dropObjectives;
	public List<KeepUpObjective> keepUpObjectives;
	public GameObject levelCompletePanel;
	public GameObject levelFailedPanel;
	public GameObject pauseMenuPanel;
	public TMPro.TMP_Text timer; // Shows the remaining time on the win timer.
	public Shooter shooter;
	//public SimulatedTrajectory projection;
	public float waitTime = 5.99f; // The amount of time the game will wait after all dropObjectives fall before giving a win.
	public AudioClip[] musicTracks;
	public AudioSource audioSource;

	public bool IsPaused { get; private set; }

	private LevelData levelData;
	private Coroutine gameWinning;
	private float remainingWaitTime;
	private int currentLevelScene;
	private float loadLevelWait = 0.2f;
	private int currentSong;

	public LevelData LevelData
	{
		get => levelData;
		set
		{
			levelData = value;
			Time.timeScale = 1;
			levelCompletePanel.SetActive(false);
			//projection.ResetScene(levelData.transform);
			shooter.Reset();
		}
	}

	public override void Awake()
	{
		base.Awake();
		//currentLevelScene = gameObject.scene.buildIndex + 1;
		//SceneManager.LoadScene(gameObject.scene.buildIndex + 1, LoadSceneMode.Additive);
		currentSong = UnityEngine.Random.Range(0, musicTracks.Length);
		StartCoroutine(PlayMusic());
		Pause();
	}

	private void Update()
	{
		//todo fix
		//if (Input.GetKeyDown(KeyCode.Escape))
		//{
		//	if (IsPaused) Resume();
		//	else
		//	{
		//		Pause();
		//		pauseMenuPanel.SetActive(true);
		//	}
		//}

	}

	public void TogglePause()
	{
		pauseMenuPanel.SetActive(!pauseMenuPanel.activeInHierarchy);
		IsPaused = !IsPaused;
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
	}

	public void Pause()
	{
		Time.timeScale = 0;
		IsPaused = true;
	}

	public void Resume()
	{
		Time.timeScale = 1;
		IsPaused = false;
	}

	public void NextLevel()
	{
		currentLevel++;
		StartCoroutine(LoadLevel(true));
	}

	public void ReloadLevel()
	{
		StartCoroutine(LoadLevel(false));
	}


	private string[] levels = { "Level_01", "Level_02", "Level_03", "Level_04", "EndOfGame" };
	private int currentLevel = 0;
	private IEnumerator LoadLevel(bool next)
	{
		dropObjectives.Clear();
		keepUpObjectives.Clear();
		Resume();
		//levelCompletePanel.SetActive(false);
		//levelFailedPanel.SetActive(false);
		yield return new WaitForSeconds(loadLevelWait);
		//SceneManager.UnloadSceneAsync(currentLevelScene);
		//if (next) currentLevelScene++;
		//SceneManager.LoadScene(currentLevelScene, LoadSceneMode.Additive);
		if (next) SceneSystem.Instance.LoadGroup(currentLevel > levels.Length-1, levels[currentLevel]);
		else SceneSystem.Instance.Reload();
	}

	internal void RegisterObjective(KeepUpObjective keepUpObjective)
	{
		keepUpObjectives.Add(keepUpObjective);
	}

	internal void RegisterObjective(DropObjective dropObjective)
	{
		dropObjectives.Add(dropObjective);
	}

	internal void DropObjective(KeepUpObjective keepUpObjective)
	{
		keepUpObjectives.Remove(keepUpObjective);
		// Can change this line to only run if all objectives are dropped, or decrement player points or something.
		if (gameWinning != null)
		{
			StopCoroutine(gameWinning);
			timer.transform.parent.gameObject.SetActive(false);
		}
		Pause();
		levelFailedPanel.SetActive(true);
	}

	internal void DropObjective(DropObjective dropObjective)
	{
		dropObjectives.Remove(dropObjective);
		if (dropObjectives.Count == 0)
		{
			gameWinning = StartCoroutine(WinGame());
		}
	}

	private IEnumerator WinGame()
	{
		timer.transform.parent.gameObject.SetActive(true);
		for (remainingWaitTime = waitTime; remainingWaitTime > 0; remainingWaitTime -= Time.deltaTime)
		{
			timer.text = remainingWaitTime.ToString("#");
			yield return new WaitForEndOfFrame();
		}
		levelCompletePanel.SetActive(true);
		Time.timeScale = 0;
		timer.transform.parent.gameObject.SetActive(false);
	}

	private IEnumerator PlayMusic()
	{
		int prevSong = currentSong;
		audioSource.PlayOneShot(musicTracks[currentSong]);
		while (currentSong == prevSong)
		{
			currentSong = UnityEngine.Random.Range(0, musicTracks.Length);
		}

		//currentSong = (currentSong + 1) % musicTracks.Length;
		yield return new WaitForSecondsRealtime(musicTracks[prevSong].length);
		StartCoroutine(PlayMusic());
	}
}
