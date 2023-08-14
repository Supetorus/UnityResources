using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Need : MonoBehaviour
{
	public enum Type
	{
		ENERGY,
		HUNGER,
		BLADDER,
		HYGIENE,
		FUN
	}

	public Type type;
	public AnimationCurve curve;
	public float input = 1;
	public float decay = 0;
	public MeterUI meter;

	public float motive { get { return GetMotive(input); } }

	private void Start()
	{
		meter.name = type.ToString();
		meter.text.text = type.ToString();
	}

	void Update()
	{
		input = input - (decay * Time.deltaTime);
		input = Mathf.Clamp(input, -1, 1);

		meter.slider.value = 1 - motive;
	}

	public float GetMotive(float value)
	{
		return Mathf.Clamp(curve.Evaluate(value), 0, 1);
	}

}
