using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(Rigidbody2D))]
public class FirePlane : Unit
{
    public float speed = 20;
    public float rotationalSpeed = 5;
    Rigidbody2D rigid;
    float rotationVel;

    public int tankThrust = 100;

    public event Action OnGoalReached;

    ParticleSystem particles;

    protected override void Start()
    {
        base.Start();
        rigid = GetComponent<Rigidbody2D>();
        particles = GetComponent<ParticleSystem>();
    }


    public void SetUp(Vector2 goalPos, Vector2 dif)
    {
        Vector2 dir = dif.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg -90;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        base.goalPosition = Vector2.zero;
        base.goalDir = Vector2.zero;

        base.goalPosition = goalPos;
        base.goalDir = dif;

        SetGoalPosition(goalPos, dif);

        print("SET UP");

    }

    public void DrawGoalPosition()
    {
        Vector2 position = goalPosition;
        Vector2 dif = goalDir;
        Vector2 right = Vector3.Cross(dif.normalized, Vector3.forward).normalized;

        Vector2 start = position - dif.normalized * radius;
        Vector2 end = position + dif + dif.normalized * radius;

        Vector2 topLeft = end - right * radius - dif.normalized * radius;
        Vector2 topRight = end + right * radius - dif.normalized * radius;
        Vector2 bottomLeft = start - right * radius;
        Vector2 bottomRight = start + right * radius;

        Vector3[] points = new Vector3[] { goalPosition, bottomLeft, topLeft, end, topRight, bottomRight, goalPosition };
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
            if (goalDir.magnitude > 0.5f)
                StartExtinguish();
            OnGoalReached?.Invoke();
        }

        if (particles.isPlaying)
            Extinguish();

        Vector2 forward = transform.TransformDirection(Vector2.up);

        float stepSize = speed * Time.deltaTime;
        Vector2 velocity = forward * stepSize;
        position += velocity;
        rigid.position = position;
    }

    public void Extinguish()
    {
        float thrusting = GetVolume(tankThrust * Time.deltaTime);
        if (thrusting > 0)
        {
            FireSystem.singleton.ExtinguishTiles(transform.position, radius, thrusting * 100);
        }

        if (tankVolume <= 0)
        {
            //Search for tank
            GoToClosestRefillStation();
        }

    }

    public void StartExtinguish()
    {
        particles.Play();
        SetGoalPosition(goalPosition + goalDir, Vector2.zero);
    }


    public void GoToClosestRefillStation()
    {
        Invoke("Refill", 10);
    }

    public void Refill()
    {
        Destroy(gameObject);
    }


}
