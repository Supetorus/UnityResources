
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank: MonoBehaviour
{
	public PlayerInfo playerInfo;

	public void AddChips(int chips)
	{
		playerInfo.bankBalance -= chips;
		playerInfo.chipBalance += chips;
	}
}
