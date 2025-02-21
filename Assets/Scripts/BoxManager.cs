using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    private bool gameCompleted = false;
    public List<BoxController> boxes = new List<BoxController>();

    public void CheckArrangement()
    {
        Debug.Log("CheckArrangement() called.");
        if (gameCompleted || !AllBoxesPlaced())
        {
            Debug.Log("Skipping check: gameCompleted=" + gameCompleted + ", allBoxesPlaced=" + AllBoxesPlaced());
            return;
        }

        int stars = CheckByType() ? 3 : CheckByPattern() ? 2 : CheckByHeight() ? 1 : 0;
        Debug.Log($"Sorting result: {stars} stars.");

        if (stars > 0)
        {
            gameCompleted = true;
            Debug.Log($"✅ Arrangement complete! Awarding {stars} stars.");
            EventBus.PublishArrangementCompleted(stars);
            boxes.ForEach(box => box.SetPlacedState(true));
        }
        else
        {
            Debug.Log("❌ No valid sorting detected.");
        }
    }

    private List<BoxController> GetPlayerArrangedBoxes()
    {
        var hooks = HookManager.Instance.GetSortedHooks();
        Debug.Log("Hooks in order:");
        foreach (var hook in hooks)
        {
            if (hook.IsOccupied())
                Debug.Log($"Hook ({hook.gridPosition.x}, {hook.gridPosition.y}) contains Box - Height: {hook.attachedBox.height}");
            else
                Debug.Log($"Hook ({hook.gridPosition.x}, {hook.gridPosition.y}) is empty.");
        }

        return hooks.Where(hook => hook.IsOccupied()).Select(hook => hook.attachedBox).ToList();
    }

    private bool CheckByHeight() =>
        CheckSorted(GetPlayerArrangedBoxes().Select(box => box.height));

    private bool CheckByPattern() =>
        CheckSorted(GetPlayerArrangedBoxes().Select(box => box.pattern));

    private bool CheckByType() =>
        GetPlayerArrangedBoxes().All(box => box.type == GetPlayerArrangedBoxes().First().type);

    private bool CheckSorted(IEnumerable<int> values)
    {
        var list = values.ToList();
        return list.Count >= 3 && (list.SequenceEqual(list.OrderBy(x => x)) || list.SequenceEqual(list.OrderByDescending(x => x)));
    }

    private bool AllBoxesPlaced()
    {
        boxes.All(box => box.isPlaced);
        foreach (var box in boxes)
        {
            Debug.Log($"Checking Box (Height: {box.height}) - isPlaced: {box.isPlaced}");
            if (!box.isPlaced)
            {
                Debug.Log("❌ Not all boxes are placed!");
                return false;
            }
        }
        Debug.Log("✅ All boxes are placed!");
        return true;
    }

    private void OnEnable()
    {
        Debug.Log("✅ BoxManager: Subscribing to ResetGameRequested event.");
        EventBus.ResetGameRequested += ResetGame;
    }

    private void OnDisable()
    {
        Debug.Log("❌ BoxManager: Unsubscribing from ResetGameRequested event.");
        EventBus.ResetGameRequested -= ResetGame;
    }

    public void ResetGame()
    {
        Debug.Log("🔄 BoxManager: Resetting all boxes...");
        gameCompleted = false;
        boxes.ForEach(box => { box.ResetPosition(); box.SetPlacedState(false); });
        HookManager.Instance.ResetHooks();
    }
}
