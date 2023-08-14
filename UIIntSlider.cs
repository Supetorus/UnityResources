using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIIntSlider : MonoBehaviour
{
	[SerializeField] Slider slider;
	[SerializeField] TMP_Text label;
	[SerializeField] TMP_Text value;
    [SerializeField] IntData data;

	private void OnValidate()
	{
		if (data != null)
		{
			label.text = data.name;
			name = data.name;
		}
	}

	private void Awake()
	{
		slider.minValue = data.min;
		slider.maxValue = data.max;

		slider.onValueChanged.AddListener(UpdateValue);
		slider.SetValueWithoutNotify(data.value);

		value.text = data.value.ToString();
	}

	private void UpdateValue(float v)
	{
		data.value = (int)v;
		value.text = v.ToString();
	}
}
