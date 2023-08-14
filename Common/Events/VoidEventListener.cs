using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Listener is added to a game object to listen to events on VoidEvent scriptable object and call subscriber methods
/// </summary>
public class VoidEventListener : MonoBehaviour
{
	[SerializeField] VoidEvent voidEvent;
	[SerializeField] UnityEvent onNotify;

	private void OnEnable()
	{
		// add Notify method to VoidEvent, method is called when VoidEvent receives an event
		voidEvent.onEvent += Notify;
	}

	private void OnDisable()
	{
		// remove Notify method to VoidEvent
		voidEvent.onEvent -= Notify;
	}

	private void Notify()
	{
		// call all subscriber methods on Notify
		onNotify?.Invoke();
	}
}
