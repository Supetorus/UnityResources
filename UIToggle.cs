using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIToggle : MonoBehaviour
{
	[SerializeField] Toggle toggle;
	[SerializeField] TMP_Text label;
    [SerializeField] BoolData data;

	private void OnValidate()
	{
		if (data != null)
		{
			name = data.name;
			label.text = data.name;
		}
	}

	private void Awake()
	{
		toggle.isOn = data.value;

		toggle.onValueChanged.AddListener(UpdateValue);
	}

	private void UpdateValue(bool v)
	{
		data.value = v;
	}
}
