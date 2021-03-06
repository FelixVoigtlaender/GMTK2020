﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager instance;

    public RectTransform tooltipRect;
    public TextMeshProUGUI textField;
    RectTransform myRect;
    public float hideTime = 0.1f;

    public float lastHideToggle;

    public bool hide = true;
    Vector2 standardPosition;
    float yVel;
    private void Awake()
    {
        instance = this;
        standardPosition = tooltipRect.localPosition;
        myRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector2 currentPos = tooltipRect.localPosition;
        Vector2 targetPos = hide ? standardPosition + myRect.sizeDelta.x * Vector2.right : standardPosition;

        currentPos.x = Mathf.SmoothDamp(currentPos.x, targetPos.x, ref yVel, hideTime);
        tooltipRect.localPosition = currentPos;


        if (Input.GetKeyDown(KeyCode.F))
            ShowText("HAllO MEIN JUNG");
    }

    public static void Toggle()
    {
        SetHidding(!instance.hide);
    }
    public static void SetHidding(bool flag)
    {
        instance.hide = flag;
        instance.lastHideToggle = Time.time;
    }
    public static void Hide()
    {
        SetHidding(true);
    }
    void DoHide()
    {
        Hide();
    }
    public static void Show()
    {
        SetHidding(false);
    }
    public static void ShowText(string text, float seconds = 3)
    {
        instance.CancelInvoke("DoHide");
        instance.textField.text = text;
        Show();
        instance.Invoke("DoHide", seconds);
    }
}
