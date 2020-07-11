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
		GetExtinguishableTiles(new Vector3(10, 10, 0), 2);
	}

	void UpdateFire()
	{
		CalculateNewFirevalues();
		TileManager.singleton.UpdateImage();
		print("updated Fires");
	}


	private void CalculateNewFirevalues()
	{
		//Cache current winddirection
		int windDirX = WindSystem.singleton.xDir;
		int windDirY = WindSystem.singleton.yDir;

		for (int x = 0; x < xSize; x++)
		{
			for (int y = 0; y < TileManager.singleton.tiles.GetLength(1); y++)
			{
				Tile tile = TileManager.singleton.tiles[x, y];
				if (tile.fireValue > 0)
				{
					//Dont calculate wind, if its a new fire (fire that started this gameTick)
					if (tile.fireValue > 70 && tile.fireValue < 240)
					{
						int windX = x + windDirX;
						int windY = y + windDirY;
						if (windX > 0 && windX < xSize &&
							windY > 0 && windY < ySize)
						{
							//Increment firevalue in cell affected by wind
							TileManager.singleton.tiles[windX, windY].changeFireValue(1);
						}
						if (x > 0 && x < xSize - 1 &&
							y > 0 && y < ySize - 1)
						{
							//start fires in adjacent cells
							TileManager.singleton.tiles[x , y + 1].changeFireValue(1);
							TileManager.singleton.tiles[x , y - 1].changeFireValue(1);
							TileManager.singleton.tiles[x + 1, y ].changeFireValue(1);
							TileManager.singleton.tiles[x - 1, y ].changeFireValue(1);
						}
					}
					//Increment firevalue in this cell
					tile.changeFireValue(fireGrowthSpeed);
				}
			}
		}
	}

	public Tile[] GetExtinguishableTiles(Vector3 worldPos, float radius)
	{
		Vector2Int center= TileManager.singleton.World2ImagePos(worldPos);
		int imageRadius = Mathf.RoundToInt(TileManager.singleton.WorldDist2ImageDist(radius));
		Tile[] tilesInRadius = new Tile[imageRadius * imageRadius * imageRadius];
		int i = 0;
		for (int x = center.x - imageRadius; x < center.x + imageRadius; x++)
		{
			for (int y = center .y- imageRadius; y < center.y + imageRadius; y++)
			{
				if (x > 0 && x < xSize - 1 &&
							y > 0 && y < ySize - 1)
				{
					Tile tile = TileManager.singleton.tiles[x, y];
					tilesInRadius[i] = tile;
					i++;
				}
			}
		}
		return tilesInRadius;
	}
}
