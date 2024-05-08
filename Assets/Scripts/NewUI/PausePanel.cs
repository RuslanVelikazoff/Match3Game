using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] 
    private Button continueButton;
    [SerializeField] 
    private Button settingsButton;
    [SerializeField] 
    private Button menuButton;

    [SerializeField]
    private GameObject settingsPanel;

    [SerializeField]
    private PauseManager pauseManager;

    [SerializeField]
    private Board board;

    private void Start()
    {
        ButtonClickAction();
    }

    private void ButtonClickAction()
    {
        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
                board.currentState = GameState.move;
                pauseManager.PauseGame();
            });
        }

        if (settingsButton != null)
        {
            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(() =>
            {
                settingsPanel.SetActive(true);
            });
        }

        if (menuButton != null)
        {
            menuButton.onClick.RemoveAllListeners();
            menuButton.onClick.AddListener(() =>
            {
                pauseManager.ExitGame();
            });
        }
    }
}
