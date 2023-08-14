using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInput : MonoBehaviour
{
	[SerializeField] TMP_Text label;
	[SerializeField] TMP_Text text;
	[SerializeField] StringData data;

	private void OnValidate()
	{
		if (data != null)
		{
			name = data.name;
			label.text = data.name;
		}
	}

	private void Update()
	{
		data.value = text.text;
	}
}
