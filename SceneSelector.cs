using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
	//[SerializeField, Tooltip("The scene name to load. If none is given will attempt to load next scene in build index order.")] private string sceneName;
	//[SerializeField, Tooltip("The scene group to load")] private string sceneGroup;

	public new string[] name;
	public float wait = 0;
	public bool unloadCurrent;

	public void LoadScene()
	{
		StartCoroutine(SceneLoading());
	}
	private IEnumerator SceneLoading()
	{
		if (wait > 0) yield return new WaitForSecondsRealtime(wait);

		if (!string.IsNullOrEmpty(name[0]))
		{
			SceneManager.LoadScene(name[0], unloadCurrent ? LoadSceneMode.Single : LoadSceneMode.Additive);
		}
		else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void LoadSceneGroup()
	{
		StartCoroutine(SceneGroupLoading());
	}

	private IEnumerator SceneGroupLoading()
	{
		if (wait > 0) yield return new WaitForSecondsRealtime(wait);

		SceneSystem.Instance.LoadGroups(unloadCurrent, name);
	}
}
