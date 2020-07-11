using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DragManager : MonoBehaviour
{
    public static Drag[] drags;


    public void Awake()
    {
        drags = new Drag[2];
        for (int i = 0; i < drags.Length; i++)
        {
            drags[i] = new Drag(i);
        }
    }

    void Update()
    {
        for (int i = 0; i < drags.Length; i++)
        {
            drags[i].UpdateDrag();
        }
    }

    public class Drag
    {
        public bool isDragging;
        public Vector2 localStart;
        public Vector2 localEnd;
        public int index = 0;

        public event Action<Drag> OnDragEnded;
        public event Action<Drag> OnDragBegan;
        public event Action<Drag> OnDrag;

        public Drag(int index)
        {
            this.index = index;
        }


        public Vector2 GetStart()
        {
            return Camera.main.ScreenToWorldPoint(localStart);
        }
        public Vector2 GetEnd()
        {
            return Camera.main.ScreenToWorldPoint(localEnd);
        }

        public void DrawDebug(Color color)
        {

            Debug.DrawLine(GetStart(), GetEnd(), color);
        }

        public void UpdateDrag()
        {


            //Mouse
            bool isMouse = Input.GetMouseButton(index) || Input.GetMouseButtonDown(index) || Input.GetMouseButtonUp(index);
            Touch mouseTouch = new Touch();
            mouseTouch.position = Input.mousePosition;
            mouseTouch.phase = Input.GetMouseButton(index) ? UnityEngine.TouchPhase.Moved : mouseTouch.phase;
            mouseTouch.phase = Input.GetMouseButtonDown(index) ? UnityEngine.TouchPhase.Began : mouseTouch.phase;
            mouseTouch.phase = Input.GetMouseButtonUp(index) ? UnityEngine.TouchPhase.Ended : mouseTouch.phase;

            if (Input.touchCount != 1 && !isMouse && !isDragging)
            {

                isDragging = false;
                return;
            }

            Touch touch = isMouse ? mouseTouch : Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                localStart = localEnd = touch.position;
                OnDragBegan?.Invoke(this);
            }
            if (isDragging && touch.phase == TouchPhase.Moved)
            {
                isDragging = true;
                localEnd = touch.position;

                //drag.DrawDebug(Color.white);

                OnDrag?.Invoke(this);
            }
            if (isDragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
            {
                localEnd = touch.position;
                isDragging = false;

                //drag.DrawDebug(Color.red);

                OnDragEnded?.Invoke(this);
            }
        }
    }
}
