using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnumData", menuName = "Data/Enum")]
public class EnumData : ScriptableObject
{
	[SerializeField] int _value;
	public string[] values;

	public int value { get => _value; set => _value = value; }
}
