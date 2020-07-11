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
	[SerializeField] int fireGrowthSpeed = 10;

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
			tiles[fireSeed.x, fireSeed.y].changeFireValue(fireGrowthSpeed);
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
					if (tile.fireValue > 70 && tile.fireValue < 240)
					{
						int windX = x + windDirX;
						int windY = y + windDirY;
						if (windX > 0 && windX < mapSideLength &&
							windY > 0 && windY < mapSideLength)
						{
							//Increment firevalue in cell affected by wind
							tiles[windX, windY].changeFireValue(1);
						}
						if (x > 0 && x < mapSideLength-1 &&
							y > 0 && y < mapSideLength-1)
						{
							//start fires in adjacent cells
						tiles[x + 1, y + 1].changeFireValue(1);
						tiles[x + 1, y - 1].changeFireValue(1);
						tiles[x - 1, y + 1].changeFireValue(1);
						tiles[x - 1, y - 1].changeFireValue(1);
						}
					}

					

					//Increment firevalue in this cell
					tile.changeFireValue(fireGrowthSpeed);
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
				Color color = gradient.Evaluate(Mathf.Lerp(0,1,Mathf.InverseLerp(-3,255, tiles[x, y].fireValue)));
				map.SetPixel(x, y, color);
			}
		}
		//Applies changed colors to Image
		map.Apply();
	}
}
