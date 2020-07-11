using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class FireTruck : Unit
{
	
    public float speed = 5;
    public float rotationalSpeed = 5;
    Rigidbody2D rigid;
    float rotationVel;

    public int tankThrust = 5;

    public event Action OnGoalReached;

    protected override void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }



    public void Update()
    {
        Vector2 position = rigid.position;
        Vector2 dif = goalPosition - position;

        if (dif.magnitude < 0.5f)
        {
            OnGoalReached?.Invoke();
            Extinguish();
            return;
        }

        Vector2 forward = transform.TransformDirection(Vector2.up);

        
        var dir = dif;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;


        rigid.rotation = Mathf.SmoothDampAngle(rigid.rotation,angle,ref rotationVel, 0.1f);

        float stepSize = Mathf.Min(speed * Time.deltaTime, dif.magnitude);
        Vector2 velocity = forward * stepSize;
        position += velocity;
        rigid.position = position;
    }

    public void Extinguish()
    {
		Tile[] extinguishableTiles = FireSystem.singleton.GetExtinguishableTiles(transform.position, radius);
		bool burningTiles = false;
		foreach (var item in extinguishableTiles)
		{
			if (item.fireValue > 0)
			{
				burningTiles = true;
				break;
			}
		}
		if (!burningTiles)
			return;

		float thrusting = GetVolume(tankThrust * Time.deltaTime);
		if (thrusting > 0)
		{
			
			
			
			foreach (var tile in extinguishableTiles)
			{
				if (tile.fireValue > 0&& tile.fireValue <250)
				{
					tile.changeFireValue(-(int)(thrusting * 100));
				}
				
				
			}
		}
		
        if (tankVolume <= 0)
        {
            //Search for tank
            GoToClosestRefillStation();
        }
    }

    public void GoToClosestRefillStation()
    {
        TankFillStation closestFill = FindClosestRefill();
        if (!closestFill)
            return;

        SetGoalPosition(closestFill.transform.position, Vector2.up);

        OnGoalReached += Refill;
    }

    public void Refill()
    {
        OnGoalReached -= Refill;
        TankFillStation closestFill = FindClosestRefill();
        if (!closestFill)
            return;
        float distance = (closestFill.transform.position -transform.position).magnitude;
        if (distance < closestFill.range)
        {

            print("Refilling");
            float fill = closestFill.GetVolume(maxTankVolume - tankVolume);
            AddVolume(fill);
            SetGoalPosition(lastGoalPosition,  Vector2.up);
        }
        else
        {
            GoToClosestRefillStation();
        }
    }
}
