using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Data/PlayerInfo")]
public class PlayerInfo : ScriptableObject
{
	public string playerName;
	public int bankBalance;
	public int chipBalance;
}
