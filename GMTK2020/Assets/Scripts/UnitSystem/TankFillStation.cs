using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Building))]
public class TankFillStation : Tank
{
    public float range = 1;

	protected override void Start()
	{
		base.Start();
		GetComponent<Building>().OnExplosion += OnExplosion;
	}

	public void OnExplosion()
	{
		Tile[] extinguishableTiles = FireSystem.singleton.GetExtinguishableTiles(transform.position, 3);
		float thrusting = GetVolume(1000);
		if (thrusting > 0)
		{
			foreach (var tile in extinguishableTiles)
			{
				if (tile!=null&&tile.fireValue > 0)
				{
					tile.changeFireValue(-(int)(thrusting * 100));
				}


			}
		}

	}
}
