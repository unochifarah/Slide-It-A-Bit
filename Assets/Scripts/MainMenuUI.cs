using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public GameObject playButton;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playButton.GetComponent<Button>().onClick.AddListener(() => gameManager.LoadNextLevel());
        //playButton.SetActive(false);
    }
}
