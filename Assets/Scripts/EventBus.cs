using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    public static event Action<int> ArrangementCompleted;
    public static event Action BoxMoved;
    public static event Action<Vector2> TouchBegan, TouchMoved;
    public static event Action TouchEnded, ResetGameRequested, LoadNextLevelRequested;

    public static void PublishArrangementCompleted(int stars)
     {
          Debug.Log($"Arrangement Completed with {stars} stars!");
          ArrangementCompleted?.Invoke(stars);
     }
    public static void PublishBoxMoved() => BoxMoved?.Invoke();
    public static void PublishTouchBegan(Vector2 pos) => TouchBegan?.Invoke(pos);
    public static void PublishTouchMoved(Vector2 pos) => TouchMoved?.Invoke(pos);
    public static void PublishTouchEnded() => TouchEnded?.Invoke();

    public static void PublishResetGameRequested()
     {
     Debug.Log("🔄 EventBus: ResetGameRequested event fired!");
     ResetGameRequested?.Invoke();
     }

    public static void PublishLoadNextLevelRequested() => LoadNextLevelRequested?.Invoke();
}
