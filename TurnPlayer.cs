using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class TurnPlayer : MonoBehaviour
{
	//[System.Serializable]
	//public struct Turn
	//{
	//	public TurnDirection direction;
	//	public Transform transform;
	//}

	[SerializeField] TurnDirection[] directions;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.GetComponent<PlayerController>().Turn(directions, transform.position);
		}
	}
}
