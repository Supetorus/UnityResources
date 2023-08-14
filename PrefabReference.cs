using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabRef", menuName = "ScriptableObjects/PrefabReference", order = 1)]
public class PrefabReference : ScriptableObject
{
	public GameObject prefab;
}
