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

        DragManager.drags[0].OnDrag += OnSelection;
        DragManager.drags[0].OnDragEnded += OnSelectionEnded;

        DragManager.drags[1].OnDrag += OnSetDirection;
        DragManager.drags[1].OnDragEnded += OnSetDirectionEnded;


    }


    public void OnSelection(DragManager.Drag drag)
    {
        Vector2 start = drag.GetStart();
        Vector2 end = drag.GetEnd();
		if ((start - end).magnitude < .2f)
			return;
        DrawBox(start, end);
        selectedUnits = CheckBoxForUnits(start, end);
    }

    public void OnSelectionEnded(DragManager.Drag drag)
    {
        lineRenderer.enabled = false;
    }


    public void OnSetDirection(DragManager.Drag drag)
    {
        Vector2 start = drag.GetStart();
        Vector2 end = drag.GetEnd();

        DrawLine(start, end);
    }
    public void OnSetDirectionEnded(DragManager.Drag drag)
    {
        Vector2 start = drag.GetStart();
        Vector2 end = drag.GetEnd();
        Vector2 dif = (end - start);
        Vector2 dir = dif.magnitude < 0.1f ? Vector2.up : dif.normalized;
        Vector2 right = Vector3.Cross(dir.normalized, Vector3.forward).normalized;
        Vector2 offset = Vector2.zero;
        if (selectedUnits.Length > 1)
        {
            offset = -right.normalized * (selectedUnits[0].radius*2 + selectedUnits.Length - 1f) / selectedUnits.Length;
        }
        for (int i = 0; i < selectedUnits.Length; i++)
        {
            selectedUnits[i].CommandPosition(start + offset + right * i * selectedUnits[i].radius*2, dif);
        }
        lineRenderer.enabled = false;
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

    public void DrawLine(Vector2 start, Vector2 end)
    {
        lineRenderer.enabled = true;

        lineRenderer.positionCount = 2;
        Vector3[] positions = new Vector3[] { start, end};
        lineRenderer.SetPositions(positions);
    }
}
