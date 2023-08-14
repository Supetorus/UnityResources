using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIButton : MonoBehaviour
{
    [SerializeField] Button button;
	[SerializeField] TMP_Text label;
	[SerializeField] VoidEvent _event;

	private void OnValidate()
	{
		if (_event != null)
		{
			name = _event.name;
			label.text = _event.name;
		}
	}

	private void Awake()
	{
		button.onClick.AddListener(OnEvent);
	}

	private void OnEvent()
	{
		_event.Notify();
	}
}
