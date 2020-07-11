using UnityEngine;

public class Unit : Tank
{
    public Vector2 goalPosition;
    public Vector2 goalDir;

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
        if(goalTarget)
            goalTarget.position = goalPosition;
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
}
