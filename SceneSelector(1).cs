using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
	[SerializeField, Tooltip("The scene name to load. If none is given will attempt to load next scene in build index order.")] private string sceneName;
	[SerializeField] private float wait = 0;

	public void LoadScene()
	{
		StartCoroutine(DoIt(wait));
	}

	private IEnumerator DoIt(float wait)
	{
		if (wait > 0) yield return new WaitForSeconds(wait);

		if (!string.IsNullOrEmpty(sceneName))
		{
			SceneManager.LoadScene(sceneName);
		}
		else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
