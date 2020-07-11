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

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        particles = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        Vector2 position = rigid.position;
        Vector2 dif = goalPosition - position;

        if (dif.magnitude < radius)
        {
            OnGoalReached?.Invoke();
            StartExtinguish();
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
        if (tankVolume > 0)
        {
            float thrust = GetVolume(tankThrust * Time.deltaTime);
            ExtinguishArea(thrust);
        }
        else
        {
            particles.Stop();
            GoToClosestRefillStation();
        }

    }

    public void StartExtinguish()
    {

        if (!Extinguishable())
        {
            SetGoalPosition(goalPosition + goalDir, Vector2.zero);
            return;
        }

        particles.Play();
        SetGoalPosition(goalPosition + goalDir, goalDir);
    }


    public void GoToClosestRefillStation()
    {
        SetGoalPosition(Vector2.down * 10, Vector2.up);

        OnGoalReached += Refill;
    }

    public void Refill()
    {
        OnGoalReached -= Refill;
        tankVolume = maxTankVolume;
        SetGoalPosition(Camera.main.transform.position, Vector2.up);
    }
}
