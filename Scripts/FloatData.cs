using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatData", menuName = "Data/Float")]
public class FloatData : ScriptableObject
{
	[SerializeField] float _value;
	public float min;
	public float max;

	public float value { get => _value; set => _value = value; }
}
