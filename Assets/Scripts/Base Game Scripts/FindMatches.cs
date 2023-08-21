using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private Board board;

    public List<GameObject> currentMatches = new List<GameObject>();

    private void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private void AddToListAndMatch(GameObject dot)
    {
        if (!currentMatches.Contains(dot))
        {
            currentMatches.Add(dot);
        }    
        dot.GetComponent<Dot>().isMatched = true;
    }

    private void GetNearbyPieces(GameObject dot1, GameObject dot2, GameObject dot3)
    {
        AddToListAndMatch(dot1);
        AddToListAndMatch(dot2);
        AddToListAndMatch(dot3);
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                GameObject currentDot = board.allDots[i, j];

                if (currentDot != null)
                {
                    Dot currentDotDot = currentDot.GetComponent<Dot>();

                    //Совпадения вправо и влево
                    if (i > 0 && i < board.Width - 1)
                    {
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];

                        if (leftDot != null && rightDot != null)
                        {
                            Dot rightDotDot = rightDot.GetComponent<Dot>();
                            Dot leftDotDot = leftDot.GetComponent<Dot>();

                            if (leftDot != null && rightDot != null)
                            {
                                if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag && CheckTag(leftDotDot, currentDotDot, rightDotDot))
                                {
                                    GetNearbyPieces(leftDot, currentDot, rightDot);
                                }
                            }
                        }
                    }

                    //Совпадения вверх и вниз
                    if (j > 0 && j < board.Height - 1)
                    {
                        GameObject upDot = board.allDots[i, j + 1];
                        GameObject downDot = board.allDots[i, j - 1];

                        if (upDot != null && downDot != null)
                        {
                            Dot downDotDot = downDot.GetComponent<Dot>();
                            Dot upDotDot = upDot.GetComponent<Dot>();

                            if (upDot != null && downDot != null)
                            {
                                if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag && CheckTag(upDotDot, currentDotDot, downDotDot))
                                {
                                    GetNearbyPieces(upDot, currentDot, downDot);
                                }
                            }
                        }
                    }
                }
            }

        }
    }

    private bool CheckTag(Dot leftDot, Dot currentDot, Dot rightDot)
    {
        if (leftDot.tag == "Adjacent Bomb" || leftDot.tag == "Color" || leftDot.tag == "Column Bomb" || leftDot.tag == "Row Bomb")
        {
            return false;
        }
        else if (currentDot.tag == "Adjacent Bomb" || currentDot.tag == "Color" || currentDot.tag == "Column Bomb" || currentDot.tag == "Row Bomb")
        {
            return false;
        }
        else if (rightDot.tag == "Adjacent Bomb" || rightDot.tag == "Color" || rightDot.tag == "Column Bomb" || rightDot.tag == "Row Bomb")
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void CheckBombs(MatchType matchType)
    {
        //Двигал ли игрок что-то?
        if (board.currentDot != null)
        {
            //Проверка совпадения
            if (board.currentDot.isMatched && board.currentDot.tag == matchType.color)
            {
                board.currentDot.isMatched = false;

                //Решаем какую бомбу создать

                if ((board.currentDot.SwipeAngle > -45 && board.currentDot.SwipeAngle <= 45)
                    || (board.currentDot.SwipeAngle < -135 || board.currentDot.SwipeAngle >= 135))
                {
                    board.currentDot.MakeRowBomb();
                }
                else
                {
                    board.currentDot.MakeRowBomb();
                }
            }

            //Проверка на совпадение другой части
            else if (board.currentDot.otherDot != null)
            {
                Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();

                //Проверка на совпадение другой точки
                if (otherDot.isMatched && otherDot.tag == matchType.color)
                {
                    otherDot.isMatched = false;

                    //Решаем какую бомбу сделать

                    if ((board.currentDot.SwipeAngle > -45 && board.currentDot.SwipeAngle <= 45)
                    || (board.currentDot.SwipeAngle < -135 || board.currentDot.SwipeAngle >= 135))
                    {
                        otherDot.MakeColumnBomb();
                    }
                    else
                    {
                        otherDot.MakeColumnBomb();
                    }
                }
            }
        }
    }
}
