using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDropdown : MonoBehaviour
{
	[SerializeField] TMP_Dropdown dropdown;
    [SerializeField] EnumData data;

	private void OnValidate()
	{
		if (data != null)
		{
			name = data.name;
		}
	}

	private void Awake()
	{
		dropdown.ClearOptions();
		dropdown.AddOptions(new List<string>(data.values));
		dropdown.onValueChanged.AddListener(UpdateValue);
		dropdown.SetValueWithoutNotify(data.value);
	}

	private void UpdateValue(int v)
	{
		data.value = v;
	}
}
