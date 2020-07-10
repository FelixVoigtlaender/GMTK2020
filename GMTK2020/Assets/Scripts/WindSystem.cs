using UnityEngine;
using System.Collections;

public class WindSystem : MonoBehaviour
{
	public static WindSystem singleton;
	public int xDir  = 0;
	public int yDir = 1;

	private void Awake()
	{
		singleton = this;
		GameManager.singleton.onNewTick += changeWindDir;
	}

	private void changeWindDir()
	{
		if (GameManager.singleton.tick % 15 == 0) {
			print("wind changing");
			xDir = Random.Range(-1, 2);
			yDir = Random.Range(-1, 2);
		}
		
	}
}
