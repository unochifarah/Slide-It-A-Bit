using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public Vector2Int gridPosition; // (row, column)
    public BoxController attachedBox;

    public bool IsOccupied() => attachedBox != null;
}