using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// mouse/touch controller
/// </summary>
public class InputController : Controller
{
    public static Action OnMouseDown;
    public static Action OnClick;
    public static Action OnDragStart;
    public static Action OnDragEnd;

    private void OnEnable()
    {
        StartCoroutine(WaitOfInput());
    }

    Vector2 startPos = new Vector2();
    Vector2 endPos = new Vector2();
    private IEnumerator WaitOfInput()
    {
        bool isDrag = false;
        while (true)
        {
            bool isButtonDown = Input.GetMouseButton(0) || Input.touchCount > 0;
            bool isDragStart = false;
            if (isButtonDown)
            {
                startPos = Input.mousePosition;
                OnMouseDown?.Invoke();
            }
               
            while (isButtonDown)
            {
                isButtonDown = Input.GetMouseButton(0) || Input.touchCount > 0;
                endPos = Input.mousePosition;

                isDrag = Vector2.Distance(startPos, endPos) > Screen.dpi;
                
                if (isDrag && !isDragStart)
                {
                    OnDragStart?.Invoke();
                    isDragStart = true;
                }
                yield return null;
            }

            if (isDrag)
            {
                isDrag = false;
                OnDragEnd?.Invoke();
            }
            else
                OnClick?.Invoke();

            yield return null;
        }
    }

}
