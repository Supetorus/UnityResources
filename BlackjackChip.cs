using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackjackChip : MonoBehaviour
{
	public int value;
	public BlackjackManager blackjackManager;

	private void OnMouseDown()
	{
		blackjackManager.AddBet(value);
	}
}
