﻿using UnityEngine;
[System.Serializable]
public class Tile
{
	public int fireValue { get; private set; } = 0;
	public int x = 0;
	public int y = 0;
	public bool isHouse = false;

	public Tile(int fireValue, int x, int y)
	{
		this.fireValue = fireValue;
		this.x = x;
		this.y = y;
	}

	public void changeFireValue(int amount)
	{
		if (fireValue <255)
			fireValue += amount;
	}
}
