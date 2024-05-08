using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameIntroPanel : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Board board;

    private void Start()
    {
        ButtonClickAction();
    }

    private void ButtonClickAction()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(() =>
            {
                board.currentState = GameState.move;
                this.gameObject.SetActive(false);
            });
        }
    }
}
