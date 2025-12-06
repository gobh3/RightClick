using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TouchEffect : MonoBehaviour
{
    public UnityEvent<Vector2> OnTap;
    private Vector2 lastT;
    // Update is called once per frame
    /*void Update()
    {
        if (Application.isMobilePlatform && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                OnTap?.Invoke(touch.position);
                //p/rint("click");
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            OnTap?.Invoke(Input.mousePosition);
            //p/rint("mouse");
        }
    }*/

    public void MakeTouchEffect()
    {
        if (Application.isMobilePlatform && Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            OnTap?.Invoke(t.position);
        }
        else
        {
            OnTap?.Invoke(Input.mousePosition);
        }
    }
}
