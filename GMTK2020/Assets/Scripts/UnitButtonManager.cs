using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButtonManager : MonoBehaviour
{
	public GameObject planePrefab;
	public GameObject truckPrefab;
	public Button planeButton;
	public Button truckButton;

	public float planeCooldown = 20;
	public float truckCooldown = 2;



	// Update is called once per frame
	void Update()
	{
		if (planeButton.interactable == false)
		{
			planeButton.image.fillAmount += 1.0f / planeCooldown * Time.deltaTime;
			if (planeButton.image.fillAmount >= 1)
			{
				planeButton.interactable = true;
			}
		}
		if (truckButton.interactable == false)
		{
			truckButton.image.fillAmount += 1.0f / truckCooldown * Time.deltaTime;
			if (truckButton.image.fillAmount >= 1)
			{
				truckButton.interactable = true;
			}
		}
	}

	public void SpawnObject(bool isPlane)
	{
        UnitManager um = FindObjectOfType<UnitManager>();

        GameObject objTospawn = null;
		if (isPlane)
		{
			objTospawn = planePrefab;
            DragManager.drags[1].OnDragEnded += SpawnPlane;
            um.selectedUnits = new Unit[0];
            return;
		}
		else
		{


			objTospawn = truckPrefab;
			truckButton.interactable = false;
			truckButton.image.fillAmount = 0;
		}

        AbortSpawnPlane(null);


		GameObject spawned = Instantiate(objTospawn, new Vector3(50, 50, 0), Quaternion.identity);
		Unit spawnedUnit = spawned.GetComponent<Unit>();
		spawnedUnit.SetGoalPosition(new Vector2(50, 50), Vector2.up);

		
		Unit[] currentSelection = um.selectedUnits;
		Unit[] newSelection = new Unit[currentSelection.Length + 1];
		for (int i = 0; i < currentSelection.Length; i++)
		{
			newSelection[i] = currentSelection[i];
		}
		newSelection[newSelection.Length - 1] = spawnedUnit;
		um.selectedUnits = newSelection;
	}


    public void AbortSpawnPlane(DragManager.Drag drag)
    {
        DragManager.drags[1].OnDragEnded -= SpawnPlane;
    }

    public void SpawnPlane(DragManager.Drag drag)
    {
        Vector2 start = drag.GetStart();
        Vector2 end = drag.GetEnd();
        Vector2 dif =end - start;

        DragManager.drags[1].OnDragEnded -= SpawnPlane;
        if ((dif).magnitude < 0.1f)
        {
            return;
        }


        planeButton.interactable = false;
        planeButton.image.fillAmount = 0;
        GameObject planeObject = Instantiate(planePrefab, start - dif.normalized * 100, Quaternion.identity);
        planeObject.GetComponent<FirePlane>().SetUp(start,dif);

    }
}
