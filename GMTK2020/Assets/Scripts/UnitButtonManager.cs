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


    LineRenderer lineRenderer;

	private void Awake()
	{
		truckButton.interactable = false;
		truckButton.image.fillAmount = 0;
		planeButton.interactable = false;
		planeButton.image.fillAmount = 0;

        lineRenderer =GetComponent<LineRenderer>();
	}
	// Update is called once per frame
	void Update()
	{
        //Sp
        if (Input.GetKeyDown(KeyCode.Alpha1) && planeButton.interactable)
            SpawnObject(false);
        if (Input.GetKeyDown(KeyCode.Alpha2) && planeButton.interactable)
            SpawnObject(true);

        if (GameManager.singleton.isPaused)
			return;
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
            SubscribeSpawnPlane();
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

		
		UnitManager.instance.AddSelectedUnit(spawnedUnit);
	}


    public void AbortSpawnPlane(DragManager.Drag drag)
    {
        DragManager.drags[1].OnDragEnded -= SpawnPlane;
        DragManager.drags[1].OnDrag -= DrawPlanePath;
        lineRenderer.enabled = false;
    }
    public void SubscribeSpawnPlane()
    {
        UnitManager um = UnitManager.instance;
        DragManager.drags[1].OnDragEnded += SpawnPlane;
        um.ClearSelectedUnits();

        //ToolTip
        ToolTipManager.ShowText("Right-Click drag to select Plane path");

        DragManager.drags[1].OnDragEnded += SpawnPlane;
        DragManager.drags[1].OnDrag += DrawPlanePath;
    }

    public void DrawPlanePath(DragManager.Drag drag)
    {
        lineRenderer.enabled = true;
        FirePlane.DrawGoalPosition(lineRenderer, drag.GetStart(), drag.GetEnd() - drag.GetStart(), 3);
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


        AbortSpawnPlane(null);
    }
}
