using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	private Dictionary<GameObject, int> inventory = new Dictionary<GameObject, int>();

	[SerializeField] private InventoryUI inventoryUI;

	private int currentKey = 1;

	public void Pickup(GameObject gameObject)
	{
		inventory.Add(gameObject, currentKey);
		inventoryUI.Add(gameObject, currentKey);
	}

	internal bool HasKey(Key key)
	{
		foreach (GameObject go in inventory.Keys)
		{
			if (go.TryGetComponent(out Key keyObject) && keyObject == key) return true;
		}
		return false;
	}
}
