using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public Button pauseButton;
    public Button backButton;

    public GameObject pausePanel;

    public bool paused = false;

    private Board board;

    private void Start()
    {
        ButtonClickAction();

        pausePanel.SetActive(false);
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
    }

    private void ButtonClickAction()
    {
        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveAllListeners();
            pauseButton.onClick.AddListener(() =>
            {
                PauseGame();
                board.currentState = GameState.pause;
                pausePanel.SetActive(true);
            });
        }

        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() =>
            {
                ExitGame();
            });
        }
    }

    public void PauseGame()
    {
        paused = !paused;
    }

    public void ExitGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
