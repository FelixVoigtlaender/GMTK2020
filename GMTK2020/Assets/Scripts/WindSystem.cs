using UnityEngine;
using System.Collections;

public class WindSystem : MonoBehaviour
{
	public static WindSystem singleton;
	public int xDir  = 0;
	public int yDir = 1;

	int maxChangeSquared = 9;
	private void Awake()
	{
		singleton = this;
		
	}
	private void Start()
	{
		GameManager.singleton.onNewTick += changeWindDir;
	}

	private void changeWindDir()
	{
		if (GameManager.singleton.tick % 10 == 0) {
			

			int newXDir = xDir+Random.Range(-1, 2);
			int newYDir = yDir + Random.Range(-1, 2);
			if(Mathf.Pow(newXDir, 2)<= maxChangeSquared)
			{
				xDir = newXDir;
			}
			if(Mathf.Pow(newYDir, 2) <= maxChangeSquared)
			{
				yDir = newYDir;
			}
		}
		print("wind changed");

	}
}
