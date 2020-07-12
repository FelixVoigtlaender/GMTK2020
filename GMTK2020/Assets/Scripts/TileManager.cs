using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
	public static TileManager singleton;

	[SerializeField] int xSize = 200;
	[SerializeField] int ySize = 200;
	[SerializeField] Image imageForMap = default;
	[SerializeField] Gradient gradient = default;

	public Texture2D map { get; private set; }
	public Tile[,] tiles { get; private set; }

	float rectTransformWidth;
	float rectTransformHeight;

	// Use this for initialization
	void Awake()
	{
		singleton = this;
		map = new Texture2D(xSize, ySize);
		map.filterMode = FilterMode.Point;

		imageForMap.material.mainTexture = map;
		tiles = new Tile[xSize, ySize];

		RectTransform transform = imageForMap.transform.parent.GetComponent<RectTransform>();
		rectTransformWidth = transform.sizeDelta.x;
		rectTransformHeight = transform.sizeDelta.y;

		//Fill rest of the map excluding already set seedpoints
		for (int x = 0; x < xSize; x++)
		{
			for (int y = 0; y < ySize; y++)
			{
				if (tiles[x, y] == null)
					tiles[x, y] = new Tile(0, x, y);
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
				color = gradient.Evaluate(Mathf.Lerp(0, 1, Mathf.InverseLerp(0, 255, tile.fireValue)));
				map.SetPixel(x, y, color);
			}
		}
		//Applies changed colors to Image
		map.Apply();
	}

	public Vector2Int World2ImagePos(Vector3 Worldpos)
	{
		Vector2Int imageCoords = new Vector2Int(Mathf.FloorToInt(Worldpos.x * (xSize / rectTransformWidth)), Mathf.FloorToInt(Worldpos.y * (ySize / rectTransformHeight)));
		return imageCoords;
	}

	public float WorldDist2ImageDist(float WorldDist)
	{
		return WorldDist * (xSize / rectTransformWidth);
	}
}
