using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HookManager : MonoBehaviour
{
    public static HookManager Instance;
    public List<HookController> hooks = new List<HookController>();

    private void Awake() => Instance = this;

    public List<HookController> GetSortedHooks() =>
        hooks.OrderBy(h => h.gridPosition.x).ThenBy(h => h.gridPosition.y).ToList();

    public void ResetHooks()
    {
        Debug.Log("🔄 HookManager: Resetting all hooks...");

        foreach (HookController hook in hooks)
        {
            hook.attachedBox = null;
        }
    }
}
