using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SceneGrouping;

public class SceneSystem : Singleton<SceneSystem>
{
	public SceneGrouping scenesGroups;

	private List<SceneInfo> loadedScenes = new List<SceneInfo>();
	private int currentLevel = 0;

	private void Start()
	{
		LoadGroup(false, scenesGroups.firstSceneGroup);
	}

	public void LoadGroup(bool unloadCurrentScenes, string groupName)
	{
		SceneGroup group = new SceneGroup();
		int level = 0;
		foreach (SceneGroup g in scenesGroups.sceneGroups)
		{
			level++;
			if (g.name == groupName)
			{
				group = g;
				currentLevel = level;
				break;
			}
		}

		List<SceneInfo> toRemove = new List<SceneInfo>();
		foreach (SceneInfo sc in loadedScenes)
		{
			if (unloadCurrentScenes)
			{
				SceneManager.UnloadSceneAsync(sc.scene);
				toRemove.Add(sc);
			}
			else
			{
				SceneManager.UnloadSceneAsync(sc.scene);
				toRemove.Add(sc);
			}
		}
		foreach (SceneInfo sc in toRemove) loadedScenes.Remove(sc);

		foreach (SceneInfo sc in group.scenes)
		{
			SceneManager.LoadScene(sc.scene, LoadSceneMode.Additive);
			loadedScenes.Add(sc);
		}
	}

	public void Reload()
	{
		foreach (SceneInfo sc in loadedScenes)
		{
			if (sc.doReload)
			{
				SceneManager.UnloadSceneAsync(sc.scene);
				SceneManager.LoadScene(sc.scene, LoadSceneMode.Additive);
			}
		}
	}

	internal void LoadGroups(bool unloadCurrent, string[] names)
	{
		foreach(string name in names)
		{
			LoadGroup(unloadCurrent, name);
		}
	}

	public void LoadNext()
	{
		LoadGroup(true, scenesGroups.sceneGroups[currentLevel + 1].name);
	}

	public void LoadPrevious()
	{

	}


}

