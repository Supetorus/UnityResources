using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles event with no parameter and sends to all event subscribers.
/// </summary>
[CreateAssetMenu(fileName = "VoidEvent", menuName = "Events/Void Event")]
public class VoidEvent : ScriptableObject
{
	public UnityAction onEvent;

	public void Notify()
	{
		onEvent?.Invoke();
	}
}
