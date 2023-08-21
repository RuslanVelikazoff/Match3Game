using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameType
{
    Moves,
}

[System.Serializable]
public class EndGameRequirements
{
    public GameType gameType;

    public int counterValue;
}

public class EndGameManager : MonoBehaviour
{
    public GameObject youWinPanel;
    public GameObject tryAgainPanel;
    public Text counter;

    [HideInInspector]public int currentCounterValue;
    private float timerSeconds;

    public EndGameRequirements requirements;

    private Board board;

    private void Start()
    {
        board = FindObjectOfType<Board>();

        SetGameType();
        SetupGame();
    }

    void SetGameType()
    {
        if (board != null)
        {
            if (board.world != null)
            {
                if (board.level < board.world.levels.Length)
                {
                    if (board.world.levels[board.level] != null)
                    {
                        requirements = board.world.levels[board.level].endGameRequirements;
                    }
                }
            }
        }
    }

    void SetupGame()
    {
        currentCounterValue = requirements.counterValue;

        counter.text = "" + currentCounterValue;
    }

    public void DecreaseCounerValue()
    {
        if (board.currentState != GameState.pause)
        {
            currentCounterValue--;
            counter.text = "" + currentCounterValue;

            if (currentCounterValue <= 0)
            {
                LoseGame();
            }
        }
    }

    public void WinGame()
    {
        youWinPanel.SetActive(true);
        board.currentState = GameState.win;
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;

        FadePanelController fadePanelController = FindObjectOfType<FadePanelController>();
        fadePanelController.GameOver();
    }

    public void LoseGame()
    {
        tryAgainPanel.SetActive(true);
        board.currentState = GameState.lose;
        Debug.Log("Вы проиграли :(");
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;

        FadePanelController fadePanelController = FindObjectOfType<FadePanelController>();
        fadePanelController.GameOver();
    }
}
