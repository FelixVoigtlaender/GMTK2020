using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireSystem : MonoBehaviour
{
	[SerializeField] Texture2D map;
	[SerializeField] Tile[] fireSeedpoints;
	[SerializeField] Gradient gradient;

	Tile[,] tiles;
	int mapSideLength;

	private void Awake()
	{
		mapSideLength = map.width;
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

		//Copy Tileset to not override values during calculation
		//(neues Feuer könnte sonst im gleichen Frame direkt ein neues Feuer entfachen)
		Tile[,] newTileSet = new Tile[tiles.GetLength(0), tiles.GetLength(1)];

		for (int x = 0; x < mapSideLength; x++)
		{
			for (int y = 0; y < mapSideLength; y++)
			{
				Tile tile = tiles[x, y];
				//Increment firevalue in cell affected by wind
				if (tile.fireValue > 0)
				{
					if (tile.fireValue != 1&& tile.fireValue < 240)
					{
						//Hacky workaround
						try
						{
							tiles[x + windDirX, y + windDirY].fireValue += 1;
						}
						catch
						{
						}
					}
					//Increment firevalue in this cell
					tile.fireValue += 10;
				}

				

				newTileSet[x, y] = tile;
			}
		}

		tiles = newTileSet;
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
