using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class FireTruck : Unit
{

    public Color circleColor = Color.blue;
	public float speed = 5;
	public float rotationalSpeed = 5;
	Rigidbody2D rigid;
	float rotationVel;

    public LineRenderer pathLine;

	public int tankThrust = 5;

	public event Action OnGoalReached;
    public ParticleSystem particles;

	protected override void Start()
	{
        base.Start();
		rigid = GetComponent<Rigidbody2D>();

        UnitManager.instance.AddSelectedUnit(this);
    }

    public void DrawGoalPosition()
    {
        if (pathLine)
        {
            pathLine.positionCount = 2;
            pathLine.SetPosition(0, transform.position);
            pathLine.SetPosition(1, goalPosition);
        }


        Vector2 position = goalPosition;

        var segments = 30;
        var pointCount = segments + 1;
        var points = new Vector3[pointCount];

        float angleOffset = -Vector2Angle(goalPosition - (Vector2)transform.position, 0);


        

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (angleOffset + i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0);
            points[i] += (Vector3)rigid.position;
        }
        
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

        Gradient colorGradient = new Gradient();
        GradientColorKey cStart = new GradientColorKey(circleColor,0);
        GradientColorKey cMid = new GradientColorKey(circleColor, tankVolume/maxTankVolume);
        GradientColorKey cMid2 = new GradientColorKey(Color.white,0.01f+ tankVolume / maxTankVolume);
        GradientColorKey cEnd = new GradientColorKey(Color.white, 1);
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0) };
        GradientColorKey[] colorKeys = new GradientColorKey[] {cStart,cMid,cMid2,cEnd };
        colorGradient.SetKeys(colorKeys, alphaKeys);
        lineRenderer.colorGradient = colorGradient;
    }

	public void Update()
	{
        lineRenderer.enabled = pathLine.enabled = isSelected;
        if (isSelected)
        {
            DrawGoalPosition();
        }


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
		var angle = Vector2Angle(dir, - 90);


		rigid.rotation = Mathf.SmoothDampAngle(rigid.rotation, angle, ref rotationVel, 0.1f);

		float stepSize = Mathf.Min(speed * Time.deltaTime, dif.magnitude);
		Vector2 velocity = forward * stepSize;
		position += velocity;
		rigid.position = position;
	}

    public static float Vector2Angle(Vector2 dir, float offset)
    {
        dir = dir.normalized;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + offset;
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
