using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireSystem : MonoBehaviour
{
	[SerializeField] Image imageForMap = default;

	[SerializeField] int mapSideLength = 50;
	[SerializeField] Tile[] fireSeedpoints;
	[SerializeField] Gradient gradient;
	[SerializeField] int fireGrowthSpeed = 5;

	Tile[,] tiles;
	Texture2D map;


	private void Awake()
	{
		map = new Texture2D(mapSideLength, mapSideLength);
		map.filterMode = FilterMode.Point;

		imageForMap.material.mainTexture = map;
		tiles = new Tile[mapSideLength, mapSideLength];

		//Fill in Seedpoints
		foreach (Tile fireSeed in fireSeedpoints)
		{
			tiles[fireSeed.x, fireSeed.y] = fireSeed;
		}

		//Fill rest of the map excluding already set seedpoints
		for (int x = 0; x < mapSideLength; x++)
		{
			for (int y = 0; y < mapSideLength; y++)
			{
				if (tiles[x, y] == null)
					tiles[x, y] = new Tile(0, x, y);
			}
		}
	}

	private void Start()
	{
		GameManager.singleton.onNewTick += UpdateFire;
	}

	void UpdateFire()
	{
		CalculateNewFirevalues();
		UpdateImage();
		print("updated Fires");
	}


	private void CalculateNewFirevalues()
	{
		//Cache current winddirection
		int windDirX = WindSystem.singleton.xDir;
		int windDirY = WindSystem.singleton.yDir;

		for (int x = 0; x < mapSideLength; x++)
		{
			for (int y = 0; y < mapSideLength; y++)
			{
				Tile tile = tiles[x, y];
				if (tile.fireValue > 0)
				{
					//Dont calculate wind, if its a new fire (fire that started this gameTick)
					if (tile.fireValue > 1 && tile.fireValue < 240)
					{
						int windX = x + windDirX;
						int windY = y + windDirY;
						if (windX > 0 && windX < mapSideLength &&
							windY > 0 && windY < mapSideLength)
						{
							//Increment firevalue in cell affected by wind
							tiles[windX, windY].fireValue += 1;
						}
					}
					//Increment firevalue in this cell
					tile.fireValue += fireGrowthSpeed;
				}
			}
		}
	}

	private void UpdateImage()
	{
		for (int x = 0; x < map.width; x++)
		{
			for (int y = 0; y < map.height; y++)
			{
				//Evaluate Gradient based on firevalue of the Tile
				Color color = gradient.Evaluate(tiles[x, y].fireValue / 255f);
				map.SetPixel(x, y, color);
			}
		}
		//Applies changed colors to Image
		map.Apply();
	}
}
