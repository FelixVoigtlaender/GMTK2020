using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireseed : MonoBehaviour
{
	public int radius = 3;
    // Start is called before the first frame update
    void Start()
    {
		Vector2Int center = TileManager.singleton.World2ImagePos(transform.position);
		int imageRadius = Mathf.RoundToInt(TileManager.singleton.WorldDist2ImageDist(radius));

		for (int x = center.x - imageRadius; x < center.x + imageRadius; x++)
		{
			for (int y = center.y - imageRadius; y < center.y + imageRadius; y++)
			{
					float dist = Vector2.SqrMagnitude(center - new Vector2(x, y));
					if (dist < imageRadius * imageRadius)
					{
					TileManager.singleton.tiles[ x, y].changeFireValue(Random.Range(2, 50));
				}
				
			}
		}
	
		//Destroy(gameObject);
	}
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
