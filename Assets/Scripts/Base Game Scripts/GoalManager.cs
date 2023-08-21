using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlankGoal
{
    public int numberNeeded;
    public int numberCollected;

    public string matchValue;

    public Sprite goalSprite;
}

public class GoalManager : MonoBehaviour
{
    public BlankGoal[] levelGoals;
    public List<GoalPanel> currentGoals = new List<GoalPanel>();

    public GameObject goalPrefab;
    public GameObject goalIntroParent;
    public GameObject goalGameParent;

    private EndGameManager endGameManager;
    private Board board;

    private void Start()
    {
        endGameManager = FindObjectOfType<EndGameManager>();
        board = FindObjectOfType<Board>();

        GetGoals();
        SetupGoals();
    }

    void GetGoals()
    {
        if (board != null)
        {
            if (board.world != null)
            {
                if (board.level < board.world.levels.Length)
                {
                    if (board.world.levels[board.level] != null)
                    {
                        levelGoals = board.world.levels[board.level].levelGoals;
                        for (int i = 0; i < levelGoals.Length; i++)
                        {
                            levelGoals[i].numberCollected = 0;
                        }
                    }
                }
            }
        }
    }

    void SetupGoals()
    {
        for (int i = 0; i < levelGoals.Length; i++)
        {
            //Создание новой панели с целями на позиции goalIntroParent
            GameObject goal = Instantiate(goalPrefab, goalIntroParent.transform.position, Quaternion.identity);
            goal.transform.SetParent(goalIntroParent.transform, false);

            //Устанавливаем изображение и текст цели
            GoalPanel panel = goal.GetComponent<GoalPanel>();
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "" + levelGoals[i].numberNeeded;

            //Создание новой панели с целями на позиции goalGameParent
            GameObject gameGoal = Instantiate(goalPrefab, goalGameParent.transform.position, Quaternion.identity);
            gameGoal.transform.SetParent(goalGameParent.transform, false);

            //Устанавливаем изображение и текст цели
            panel = gameGoal.GetComponent<GoalPanel>();
            currentGoals.Add(panel);
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "" + levelGoals[i].numberNeeded;

        }
    }

    public void UpdateGoals()
    {
        int goalsCompleted = 0;

        for (int i = 0; i < levelGoals.Length; i++)
        {
            int goalsLeft = levelGoals[i].numberNeeded - levelGoals[i].numberCollected;
            currentGoals[i].thisText.text = "" + goalsLeft;

            if (levelGoals[i].numberCollected >= levelGoals[i].numberNeeded)
            {
                goalsCompleted++;
                currentGoals[i].thisText.text = "0";
            }
        }

        if (goalsCompleted >= levelGoals.Length)
        {
            if (endGameManager != null)
            {
                endGameManager.WinGame();
            }
        }
    }

    public void CompareGoal(string goalToCompare)
    {
        for (int i = 0; i < levelGoals.Length; i++)
        {
            if (goalToCompare == levelGoals[i].matchValue)
            {
                levelGoals[i].numberCollected++;
            }
        }
    }
}
