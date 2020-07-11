﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireseed : MonoBehaviour
{
	public int startValue = 5;
    // Start is called before the first frame update
    void Start()
    {
		Vector2Int imagePos = TileManager.singleton.World2ImagePos(transform.position);
		TileManager.singleton.tiles[imagePos.x, imagePos.y].changeFireValue (startValue);
		Destroy(gameObject);
	}
	
}