using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StringData", menuName = "Data/String")]
public class StringData : ScriptableObject
{
	[SerializeField] string _value;

	public string value { get => _value; set => _value = value; }
}

