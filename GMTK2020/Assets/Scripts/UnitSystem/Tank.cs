using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [Header("Tank")]
    public float maxTankVolume = 100;
    public float tankVolume;
    [Header("Visualization")]
    public Transform volumeTransform;

    protected virtual void Start()
    {
        tankVolume = maxTankVolume;
    }

    private void Update()
    {
        if (!volumeTransform)
            return;

        tankVolume = Mathf.Clamp(tankVolume, 0, maxTankVolume);

        Vector2 size = volumeTransform.localScale;
        size.y = tankVolume / maxTankVolume;
        volumeTransform.localScale = size;
    }

    public float GetVolume(float volume)
    {
        volume = Mathf.Min(tankVolume, volume);
        tankVolume -= volume;
        return volume;
    }
    public float AddVolume(float volume)
    {
        float filled = Mathf.Min(maxTankVolume - tankVolume, volume);
        tankVolume += filled;
        return volume - filled;
    }

    public bool IsEmpty()
    {
        return tankVolume <= 0;
    }
}
