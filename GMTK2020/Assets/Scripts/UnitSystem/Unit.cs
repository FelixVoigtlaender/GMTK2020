﻿using UnityEngine;

public class Unit : Tank
{
    public Vector2 goalPosition;
    public Vector2 goalDir;
	public float radius = 2;
	protected Vector2 lastGoalPosition;

    public Transform goalTarget;

    private void Awake()
    {
        tankVolume = maxTankVolume;
        SetGoalPosition(transform.position, Vector2.up);
    }


    public void SetGoalPosition(Vector2 goalPosition, Vector2 goalDir)
    {
        lastGoalPosition = this.goalPosition;
        this.goalPosition = goalPosition;
        this.goalDir = goalDir;
        if(goalTarget)
            goalTarget.position = goalPosition;
    }


    public void ExtinguishArea(float thrust)
    {
        if(!FireSystem.singleton.ExtinguishTiles(transform.position, radius, thrust))
		{
			AddVolume(thrust);
		}
    }


    public TankFillStation FindClosestRefill()
    {
        TankFillStation[] fillStations = FindObjectsOfType(typeof(TankFillStation)) as TankFillStation[];
        if (fillStations.Length == 0)
            return null;
        TankFillStation closestFull = fillStations[Random.Range(0,fillStations.Length)];
        float closestDistance = (closestFull.transform.position - transform.position).magnitude;
        foreach (TankFillStation fillStation in fillStations)
        {
            float distance = (fillStation.transform.position - transform.position).magnitude;
            if (!fillStation.IsEmpty() && distance < closestDistance)
            {
                closestFull = fillStation;
            }
        }

        return closestFull;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(goalPosition, Vector3.one);
        Gizmos.DrawRay(goalPosition, goalDir);
    }
}
