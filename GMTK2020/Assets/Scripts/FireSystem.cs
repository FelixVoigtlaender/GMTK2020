﻿using System.Collections.Generic;
using UnityEngine;

public class FireSystem : MonoBehaviour
{
	public static FireSystem singleton;
	[SerializeField] int fireGrowthSpeed = 10;
	int xSize;
	int ySize;

	private void Awake()
	{
		singleton = this;
	}
	private void Start()
	{
		xSize = TileManager.singleton.tiles.GetLength(0);
		ySize = TileManager.singleton.tiles.GetLength(1);
		GameManager.singleton.onNewTick += UpdateFire;
	}

	void UpdateFire()
	{
		if (!CalculateNewFirevalues())//returns false, if no flames left
		{
			print("NO FLAMES LEFT! GAME IS OVER");
		}
		TileManager.singleton.UpdateImage();
		print("updated Fires");
	}


	private bool CalculateNewFirevalues()
	{
		//Cache current winddirection
		int windDirX = WindSystem.singleton.xDir;
		int windDirY = WindSystem.singleton.yDir;
		bool fireIsActive = false;
		for (int x = 0; x < xSize; x++)
		{
			for (int y = 0; y < TileManager.singleton.tiles.GetLength(1); y++)
			{
				Tile tile = TileManager.singleton.tiles[x, y];
				if (tile.fireValue > 0)
				{
					fireIsActive = true;
					//Dont calculate wind, if its a new fire (fire that started this gameTick)
					if (tile.fireValue > 70 && tile.fireValue < 240)
					{
						int windX = x + windDirX;
						int windY = y + windDirY;
						if (windX > 0 && windX < xSize &&
							windY > 0 && windY < ySize)
						{
							//Increment firevalue in cell affected by wind
							TileManager.singleton.tiles[windX, windY].changeFireValue(Random.Range(1, 15));
						}
						if (x > 0 && x < xSize - 1 &&
							y > 0 && y < ySize - 1)
						{
							int val = Random.Range(0, 101);
							if (val < 40)
							{
								//start fires in adjacent cells
								TileManager.singleton.tiles[x, y + 1].changeFireValue(Random.Range(1, 15));
								TileManager.singleton.tiles[x, y - 1].changeFireValue(Random.Range(1, 15));
								TileManager.singleton.tiles[x + 1, y].changeFireValue(Random.Range(1, 15));
								TileManager.singleton.tiles[x - 1, y].changeFireValue(Random.Range(1, 15));
							}

						}
					}
					//Increment firevalue in this cell
					tile.changeFireValue(fireGrowthSpeed);
				}
			}
		}
		return fireIsActive;
	}

	public bool ExtinguishTiles(Vector3 worldPos, float radius, float amount)
	{
		Vector2Int center = TileManager.singleton.World2ImagePos(worldPos);
		int imageRadius = Mathf.RoundToInt(TileManager.singleton.WorldDist2ImageDist(radius));
		int i = 0;
		bool didExtinguish = false;
		for (int x = center.x - imageRadius; x < center.x + imageRadius; x++)
		{
			for (int y = center.y - imageRadius; y < center.y + imageRadius; y++)
			{
				if (x > 0 && x < xSize - 1 && y > 0 && y < ySize - 1)
				{
					float dist = Vector2.SqrMagnitude(center - new Vector2(x, y));
					if (dist < imageRadius*imageRadius)
					{
						Tile tile = TileManager.singleton.tiles[x, y];
						if (tile.fireValue > 0 && tile.fireValue<250)
						{
							tile.changeFireValue(-(int)amount);
							didExtinguish = true;
						}
					}
					i++;
				}
			}
		}
		return didExtinguish;
	}
}
