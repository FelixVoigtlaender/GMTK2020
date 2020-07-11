using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArrow : MonoBehaviour
{
    public void UpdateWindArrowRotation()
	{
		transform.rotation = rotateWithWinddirection();
	}

	public Quaternion rotateWithWinddirection()
	{
		Vector3 windvector = new Vector3(WindSystem.singleton.xDir, WindSystem.singleton.yDir, 0);
		return Quaternion.FromToRotation(Vector3.up, windvector);
	}
}
