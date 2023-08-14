using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashOut : MonoBehaviour
{
	public PlayerInfo playerInfo;

	public void Cash()
	{
		playerInfo.bankBalance += playerInfo.chipBalance;
		playerInfo.chipBalance = 0;
	}
}
