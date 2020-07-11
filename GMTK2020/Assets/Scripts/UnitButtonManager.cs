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
		GameObject objTospawn = null;
		if (isPlane)
		{
			objTospawn = planePrefab;
			planeButton.interactable = false;
			planeButton.image.fillAmount = 0;
		}
		else
		{
			objTospawn = truckPrefab;
			truckButton.interactable = false;
			truckButton.image.fillAmount = 0;
		}

		GameObject spawned = Instantiate(objTospawn, new Vector3(10, 10, 0), Quaternion.identity);
		Unit spawnedUnit = spawned.GetComponent<Unit>();
		spawnedUnit.SetGoalPosition(new Vector2(50, 50), Vector2.up);

		UnitManager um = FindObjectOfType<UnitManager>();
		Unit[] currentSelection = um.selectedUnits;
		Unit[] newSelection = new Unit[currentSelection.Length + 1];
		for (int i = 0; i < currentSelection.Length; i++)
		{
			newSelection[i] = currentSelection[i];
		}
		newSelection[newSelection.Length - 1] = spawnedUnit;
		um.selectedUnits = newSelection;
	}
}
