using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolData", menuName = "Data/Bool")]
public class BoolData : ScriptableObject
{
	[SerializeField] bool _value;

	public bool value { get => _value; set => _value = value; }
}

