using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetName : MonoBehaviour
{
	public TMP_InputField userName;
	public PlayerInfo playerInfo;

	public void Run()
	{
		playerInfo.playerName = userName.text;
	}
}
