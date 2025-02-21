using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
         #if UNITY_EDITOR
         // for testing in editor using mouse input:
         if (Input.GetMouseButtonDown(0))
         {
              Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
              EventBus.PublishTouchBegan(pos);
         }
         if (Input.GetMouseButton(0))
         {
              Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
              EventBus.PublishTouchMoved(pos);
         }
         if (Input.GetMouseButtonUp(0))
         {
              EventBus.PublishTouchEnded();
         }
         #else
         // mobile touch input:
         if (Input.touchCount > 0)
         {
              Touch touch = Input.GetTouch(0);
              Vector2 pos = Camera.main.ScreenToWorldPoint(touch.position);
              if (touch.phase == TouchPhase.Began)
              {
                   EventBus.PublishTouchBegan(pos);
              }
              else if (touch.phase == TouchPhase.Moved)
              {
                   EventBus.PublishTouchMoved(pos);
              }
              else if (touch.phase == TouchPhase.Ended)
              {
                   EventBus.PublishTouchEnded();
              }
         }
         #endif
    }
}
