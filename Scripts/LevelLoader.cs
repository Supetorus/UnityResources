using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
	public void LoadLevel(string level)
	{
		SceneSystem.Instance.LoadGroup(true, level);
	}
}
