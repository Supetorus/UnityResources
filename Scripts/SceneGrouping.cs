using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneGroups", menuName = "Data/SceneGrouping")]
public class SceneGrouping : ScriptableObject
{
	public SceneGroup[] sceneGroups;
	public string firstSceneGroup;

	[System.Serializable]
	public struct SceneGroup
	{
		public string name;
		public SceneInfo[] scenes;
	}

	[System.Serializable]
	public struct SceneInfo
	{
		public string scene;
		public bool doReload;
	}
}
