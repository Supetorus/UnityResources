using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
	private void Awake()
	{
		Game.Instance.LevelData = this;
	}
}
