using UnityEngine;
using System.Collections;

public class WindSystem : MonoBehaviour
{
	public static WindSystem singleton;
	public int xDir = 2;
	public int yDir = -2;

    public int windChangeTime = 10;

	int maxChangeSquared = 9;
	WindArrow arrow;
	private void Awake()
	{
		singleton = this;
		arrow = GameObject.FindObjectOfType<WindArrow>();
	}
	private void Start()
	{
		GameManager.singleton.onNewTick += changeWindDir;
	}

	private void changeWindDir()
	{
		if (GameManager.singleton.tick % windChangeTime == 0)
		{
			int newXDir = xDir;
			int newYDir = yDir;
			while (newXDir <= 0)
			{
				newXDir = xDir + Random.Range(-1, 2);
			}
			newYDir = yDir + Random.Range(-1, 2);

			if (Mathf.Pow(newXDir, 2) <= maxChangeSquared)
			{
				xDir = newXDir;
			}
			if (Mathf.Pow(newYDir, 2) <= maxChangeSquared)
			{
				yDir = newYDir;
			}
		}
		arrow.UpdateWindArrowRotation();
		//print("wind changed: "+xDir+","+yDir);

	}
}
