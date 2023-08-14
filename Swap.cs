using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class Swap : InputProcessor<Vector2>
{
#if UNITY_EDITOR
	static Swap()
	{
		Initialize();
	}
#endif

	[RuntimeInitializeOnLoadMethod]
	static void Initialize()
	{
		InputSystem.RegisterProcessor<Swap>();
	}

	public override Vector2 Process(Vector2 value, InputControl control)
	{
		float temp = value.x;
		value.x = value.y;
		value.y = temp;
		return value;
	}
}