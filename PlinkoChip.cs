using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlinkoChip : MonoBehaviour
{
	public int value;
	public PlinkoManager manager;

	private void OnMouseDown()
	{
		manager.AddBet(value);
	}
}
