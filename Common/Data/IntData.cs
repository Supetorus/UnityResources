using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntData", menuName = "Data/Int")]
public class IntData : ScriptableObject
{
	[SerializeField] int _value;
	public float min;
	public float max;

	public int value { get => _value; set => _value = value; }
}
