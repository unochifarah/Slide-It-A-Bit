using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private BoxManager boxManager;
    private UIManager uiManager;

    private void Start() => boxManager = ServiceLocator.BoxManager;

    private void Awake()
    {
        var boxManager = FindObjectOfType<BoxManager>();
        var uiManager = FindObjectOfType<UIManager>();

        Debug.Log("🔍 GameManager: Registering services...");
        if (boxManager == null) Debug.LogError("❌ GameManager: BoxManager is missing from the scene!");
        if (uiManager == null) Debug.LogError("❌ GameManager: UIManager is missing from the scene!");

        ServiceLocator.Register(this, boxManager, uiManager);

        Debug.Log("✅ GameManager: UIManager in ServiceLocator = " + (ServiceLocator.UIManager != null));
    }

    private void OnEnable()
    {
        Debug.Log("Subscribing to events...");
        EventBus.BoxMoved += HandleBoxMove;
        EventBus.ArrangementCompleted += HandleArrangementCompleted;
    }

    private void OnDisable()
    {
        Debug.Log("Unsubscribing from events...");
        EventBus.BoxMoved -= HandleBoxMove;
        EventBus.ArrangementCompleted -= HandleArrangementCompleted;
    }

    private void HandleBoxMove()
    {
        Debug.Log("HandleBoxMove() triggered.");
        if (boxManager == null)
        {
            Debug.LogError("boxManager is NULL!");
            return;
        }
        Debug.Log("Calling CheckArrangement()...");
        boxManager.CheckArrangement();
    }

    private void HandleArrangementCompleted(int stars)
    {
        Debug.Log($"🎯 GameManager: Received arrangement completed event! Stars: {stars}");
        ServiceLocator.UIManager.ShowStars(stars);
    }

    public void ResetGame()
    {
        Debug.Log("🔄 GameManager: Resetting game...");

        if (ServiceLocator.UIManager == null)
        {
            Debug.LogError("❌ ServiceLocator.UIManager is NULL! ServiceLocator may not be registering correctly.");
            return;
        }

        Debug.Log("✅ GameManager: UIManager found, hiding stars.");
        ServiceLocator.UIManager.HideStars();

        EventBus.PublishResetGameRequested();
    }

    public void LoadNextLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    public void LoadMenuScene() => SceneManager.LoadScene("MAINMENU");
}
