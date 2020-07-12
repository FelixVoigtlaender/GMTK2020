using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    LineRenderer lineRenderer;
    public LayerMask unitLayer;

    public List<Unit> selectedUnits;

    public void Awake()
    {
        instance = this;
    }
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
        if ((start - end).magnitude < 0.1f)
            end += Vector2.one * 0.1f;

        SetSelectionFlag(selectedUnits, false);

        selectedUnits = CheckBoxForUnits(start, end);

        SetSelectionFlag(selectedUnits, true);

        DrawBox(start, end);
    }

    public void OnSelectionEnded(DragManager.Drag drag)
    {
        lineRenderer.enabled = false;
    }

    public void AddSelectedUnit(Unit unit)
    {
        selectedUnits.Add(unit);
        unit.isSelected = true;
    }
    public void RemoveSelectedUnit(Unit unit)
    {
        selectedUnits.Remove(unit);
        unit.isSelected = false;
    }
    public void ClearSelectedUnits()
    {
        SetSelectionFlag(selectedUnits, false);
        selectedUnits.Clear();
    }

    public void SetSelectionFlag(List<Unit> units, bool flag)
    {
        foreach (Unit selection in units)
        {
            selection.isSelected = flag;
        }
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
        if (selectedUnits.Count > 1)
        {
            offset = -right.normalized * (selectedUnits[0].radius*selectedUnits.Count) / 2;
        }

        for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].CommandPosition(start + offset + right * i * selectedUnits[i].radius*2, dif);
        }
        lineRenderer.enabled = false;
    }

    public List<Unit> CheckBoxForUnits(Vector2 start, Vector2 end)
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

        return units;
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
