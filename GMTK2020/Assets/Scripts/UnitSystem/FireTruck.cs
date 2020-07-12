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
    public ParticleSystem particles;

	protected override void Start()
	{
		rigid = GetComponent<Rigidbody2D>();
    }

    public void DrawGoalPosition()
    {
        Vector2 position = goalPosition;

        var segments = 30;
        var pointCount = segments + 1;
        var points = new Vector3[pointCount+2];
        points[0] = transform.position;
        points[1] = goalPosition;

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i+2] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0);
            points[i+2] += (Vector3)goalPosition;
        }


        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

	public void Update()
	{
        DrawGoalPosition();

		Vector2 position = rigid.position;
		Vector2 dif = goalPosition - position;

        if (dif.magnitude < 0.5f)
		{
			OnGoalReached?.Invoke();

            Extinguish();
			return;
        }
        else
        {
            particles.Stop();
        }

		Vector2 forward = transform.TransformDirection(Vector2.up);


		var dir = dif;
		var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;


		rigid.rotation = Mathf.SmoothDampAngle(rigid.rotation, angle, ref rotationVel, 0.1f);

		float stepSize = Mathf.Min(speed * Time.deltaTime, dif.magnitude);
		Vector2 velocity = forward * stepSize;
		position += velocity;
		rigid.position = position;
	}

	public void Extinguish()
	{
		float thrusting = GetVolume(tankThrust * Time.deltaTime);
		if (thrusting > 0)
		{
			if(!FireSystem.singleton.ExtinguishTiles(transform.position, radius, thrusting * 100))
			{
				AddVolume(thrusting);
            }
            else
            {
                particles.Play();
                particles.startSpeed =radius*2* tankVolume/maxTankVolume;
                Vector3 rotation = particles.transform.rotation.eulerAngles;
                rotation.z += 30 * UnityEngine.Random.Range(-1,2); 
                particles.transform.rotation =Quaternion.Euler(rotation);
            }
        }
        else
        {
            particles.Stop();
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
		float distance = (closestFill.transform.position - transform.position).magnitude;
		if (distance < closestFill.range)
		{

			print("Refilling");
			float fill = closestFill.GetVolume(maxTankVolume - tankVolume);
			AddVolume(fill);
			SetGoalPosition(lastGoalPosition, Vector2.up);
		}
		else
		{
			GoToClosestRefillStation();
		}
	}
}
