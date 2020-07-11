using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    LineRenderer lineRenderer;
    public LayerMask unitLayer;

    public Unit[] selectedUnits;

    public void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        DragManager.OnDrag += OnDrag;
        DragManager.OnDragEnded += OnDragEnded;
    }

    public void OnDrag(DragManager.Drag drag)
    {
        Vector2 start = drag.GetStart();
        Vector2 end = drag.GetEnd();

        DrawBox(start, end);
        selectedUnits = CheckBoxForUnits(start, end);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            print("CLICK");
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            for (int i = 0; i < selectedUnits.Length; i++)
            {
                selectedUnits[i].SetGoalPosition(position, Vector2.up);
            }
        }
    }

    public Unit[] CheckBoxForUnits(Vector2 start, Vector2 end)
    {
        Vector2 dif = end - start;
        Collider2D[] hits = Physics2D.OverlapBoxAll(start + dif / 2, new Vector2(Mathf.Abs(dif.x), Mathf.Abs(dif.y)), 0, unitLayer);

        List<Unit> units = new List<Unit>();
        foreach(Collider2D hit in hits)
        {
            Unit unit = hit.GetComponent<Unit>();
            if (unit)
                units.Add(unit);
        }

        return units.ToArray();
    }

    public void OnDragEnded(DragManager.Drag drag)
    {
        lineRenderer.enabled = false;
    }


    public void DrawBox(Vector2 start, Vector2 end)
    {
        lineRenderer.enabled = true;

        Vector2 dif = start - end;

        Vector2 one = end;
        Vector2 two = end + Vector2.right * dif.x;
        Vector2 three = end + dif;
        Vector2 four = end + Vector2.up * dif.y;

        lineRenderer.positionCount = 4;
        Vector3[] positions = new Vector3[] { one, two, three, four };
        lineRenderer.SetPositions(positions);
    }
}
