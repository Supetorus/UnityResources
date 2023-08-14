using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
	public Sprite sprite;
	public int number;
	public string name;
	public int captured;

	public Player(Sprite sprite, string name, int number)
	{
		this.sprite = sprite;
		this.name = name;
		this.number = number;
		captured = 0;
	}
}
