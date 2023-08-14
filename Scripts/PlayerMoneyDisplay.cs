using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoneyDisplay : MonoBehaviour
{
	public PlayerInfo playerInfo;

	//This is the hackiest thing I've ever written.
	public int cash0Chips1;

	private TMPro.TMP_Text display;

	private void Awake()
	{
		display = GetComponent<TMPro.TMP_Text>();
	}

	private void Update()
	{
		switch(cash0Chips1)
		{
			case 0:
			display.text = $"${playerInfo.bankBalance}";
				break;
			case 1:
				display.text = $"${playerInfo.chipBalance}";
				break;
		}
	}
}
