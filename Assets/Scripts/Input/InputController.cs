using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
    public UnityEvent OnRightClicked;
    public UnityEvent OnLeftClicked;
    // Update is called once per frame
    void Update()
    {
        if (!Application.isMobilePlatform)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnRightClicked?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnLeftClicked?.Invoke();
            }
        }
    }

    private bool isTouching(string side)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 cameraPos = Camera.main.WorldToScreenPoint(touch.position);
            if (side == "right" && cameraPos.x > 0)
                return true;
            else if (side == "left" && cameraPos.x < 0)
                return true;
        }
        return false;
    }

}
