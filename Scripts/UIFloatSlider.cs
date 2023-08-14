using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFloatSlider : MonoBehaviour
{
	[SerializeField] Slider slider;
	[SerializeField] TMP_Text label;
	[SerializeField] TMP_Text value;
    [SerializeField] FloatData data;

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

		value.text = data.value.ToString("F1");
	}

	private void UpdateValue(float v)
	{
		data.value = v;
		value.text = v.ToString("F1");
	}
}
