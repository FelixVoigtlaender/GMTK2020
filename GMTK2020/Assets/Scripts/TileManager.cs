using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
	public static TileManager singleton;

	public static int mapSideLength = 50;
	
	public Texture2D map { get; private set; }
	public Tile[,] tiles { get; private set; }
	
	[SerializeField] Image imageForMap = default;
	[SerializeField] Gradient gradient;

	// Use this for initialization
	void Awake()
	{
		singleton = this;
		map = new Texture2D(mapSideLength, mapSideLength);
		map.filterMode = FilterMode.Point;

		imageForMap.material.mainTexture = map;
		tiles = new Tile[mapSideLength, mapSideLength];



		//Fill rest of the map excluding already set seedpoints
		for (int x = 0; x < mapSideLength; x++)
		{
			for (int y = 0; y < mapSideLength; y++)
			{
				if (tiles[x, y] == null)
					tiles[x, y] = new Tile(0,x, y);
			}
		}
	}

	public void UpdateImage()
	{
		for (int x = 0; x < map.width; x++)
		{
			for (int y = 0; y < map.height; y++)
			{
				//Evaluate Gradient based on firevalue of the Tile
				Tile tile = tiles[x, y];
				Color color = Color.white;
				if (tile.isHouse)
				{
					color = Color.blue;
				}
				else
				{
					 color = gradient.Evaluate(Mathf.Lerp(0, 1, Mathf.InverseLerp(-3, 255, tile.fireValue)));
				}
				
				map.SetPixel(x, y, color);
			}
		}
		//Applies changed colors to Image
		map.Apply();
	}

	public static Vector2Int World2ImagePos(Vector3 Worldpos)
	{
		Vector2Int imageCoords = new Vector2Int(Mathf.FloorToInt(Worldpos.x* (mapSideLength / 300f)), Mathf.FloorToInt(Worldpos.y* (mapSideLength / 300f)));
		return imageCoords;
	}
}
