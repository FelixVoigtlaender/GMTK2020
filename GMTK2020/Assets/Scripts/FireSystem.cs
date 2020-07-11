using UnityEngine;

public class FireSystem : MonoBehaviour
{
	[SerializeField] int fireGrowthSpeed = 10;
	

	private void Start()
	{
		GameManager.singleton.onNewTick += UpdateFire;
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
		int mapSideLength = TileManager.singleton.mapSideLength;
		
		for (int x = 0; x < mapSideLength; x++)
		{
			for (int y = 0; y < mapSideLength; y++)
			{
				Tile tile = TileManager.singleton.tiles[x, y];
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
							TileManager.singleton.tiles[windX, windY].changeFireValue(1);
						}
						if (x > 0 && x < mapSideLength-1 &&
							y > 0 && y < mapSideLength-1)
						{
							//start fires in adjacent cells
							TileManager.singleton.tiles[x + 1, y + 1].changeFireValue(1);
							TileManager.singleton.tiles[x + 1, y - 1].changeFireValue(1);
							TileManager.singleton.tiles[x - 1, y + 1].changeFireValue(1);
							TileManager.singleton.tiles[x - 1, y - 1].changeFireValue(1);
						}
					}
					//Increment firevalue in this cell
					tile.changeFireValue(fireGrowthSpeed);
				}
			}
		}
	}

	
}
