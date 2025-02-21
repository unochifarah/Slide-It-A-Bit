using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject oneStarUI;
    public GameObject twoStarUI;
    public GameObject threeStarUI;
    public GameObject replayButton;
    public GameObject continueButton;

    private GameManager gameManager;

    private void Awake()
    {
        var boxManager = FindObjectOfType<BoxManager>();
        var uiManager = FindObjectOfType<UIManager>();

        if (boxManager == null) Debug.LogError("❌ GameManager: BoxManager is missing from the scene!");
        if (uiManager == null) Debug.LogError("❌ GameManager: UIManager is missing from the scene!");
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        replayButton.GetComponent<Button>().onClick.AddListener(() => {
            Debug.Log("🔄 Replay button clicked! Calling ResetGame...");
            gameManager.ResetGame();
        });
        replayButton.SetActive(false);
        continueButton.GetComponent<Button>().onClick.AddListener(() => gameManager.LoadNextLevel());
        continueButton.SetActive(false);
    }

    public void ShowStars(int stars)
    {
        Debug.Log($"🌟 UIManager: Displaying {stars} stars!");
        
        oneStarUI.SetActive(stars == 1);
        twoStarUI.SetActive(stars == 2);
        threeStarUI.SetActive(stars == 3);
        
        replayButton.SetActive(true);
        continueButton.SetActive(true);
    }

    public void HideStars()
    {
        Debug.Log("🛑 UIManager: Hiding win UI...");
        
        oneStarUI?.SetActive(false);
        twoStarUI?.SetActive(false);
        threeStarUI?.SetActive(false);
        replayButton?.SetActive(false);
        continueButton?.SetActive(false);
    }
}