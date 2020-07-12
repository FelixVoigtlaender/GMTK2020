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
		 
		float thrusting = GetVolume(1000);
		if (thrusting > 0)
		{
			FireSystem.singleton.ExtinguishTiles(transform.position, 3,thrusting*100);
		}

	}
}
