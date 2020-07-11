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

    public void Update()
    {
        Vector2 position = rigid.position;
        Vector2 dif = goalPosition - position;

        if (dif.magnitude < radius)
        {
            if (goalDir.magnitude > 0.5f)
                StartExtinguish();
            OnGoalReached?.Invoke();
        }

        if (particles.isPlaying)
            Extinguish();

        Vector2 forward = transform.TransformDirection(Vector2.up);


        var dir = dif;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        rigid.rotation = Mathf.SmoothDampAngle(rigid.rotation, angle, ref rotationVel, 0.5f);

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
        SetGoalPosition(Vector2.down * 10, Vector2.zero);
        particles.Stop();

        OnGoalReached += Refill;
    }

    public void Refill()
    {
        OnGoalReached -= Refill;
        tankVolume = maxTankVolume;
        particles.Stop();


        SetGoalPosition(Camera.main.transform.position, Vector2.zero);
    }

    
}
