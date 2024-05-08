using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotAnimation : MonoBehaviour
{
    [Header("Анимация для столбцовой бомбы")]
    public GameObject columnRocketUp;
    public GameObject columnRocketDown;
    public float columnAnimationDuration = 3.5f;

    [Space(8)]
    [Header("Анимация для рядовой бомбы")]
    public GameObject rowRocketRight;
    public GameObject rowRocketLeft;
    public float rowAnimationDuration = 3.5f;

    [Space(8)]
    [Header("Анимация для рядово-столбцовой бомбы")]
    public float columnRowAnimationDuration = 3.5f;

    [Space(8)]
    [Header("Анимация для большой столбцовой бомбы")]
    public float megaColumnAnimationDuration = 3.5f;

    [Space(8)]
    [Header("Анимация для большой рядовой бомбы")]
    public float megaRowAnimationDuration = 3.5f;

    [Space(8)]
    [Header("Анимация для квадратной бомбы")]
    public float adjacentBombAnimationDuration = 3.5f;

    Vector2 dotPosition;

    private Board board;

    private void Start()
    {
        board = FindObjectOfType<Board>();
    }

    #region BombsMethod
    public void ColumnBombAnimation(int column, int row)
    {
        StartCoroutine(ColumnBombAnimationCO(column, row));
    }

    public void RowBombAnimation(int column, int row)
    {
        StartCoroutine(RowBombAnimationCO(column, row));
    }

    public void ColumnRowBombAnimation(int column, int row)
    {
        StartCoroutine(ColumnRowBombAnimationCO(column, row));
    }

    public void MegaColumnBombAnimation(int column, int row)
    {
        StartCoroutine(MegaColumnBombAnimationCO(column, row));
    }

    public void MegaRowBombAnimation(int column, int row)
    {
        StartCoroutine(MegaRowBombAnimationCO(column, row));
    }

    public void AdjacentBombAnimation(int column, int row)
    {
        StartCoroutine(AdjacentBombAnimationCO(column, row));
    }
    #endregion

    private IEnumerator ColumnBombAnimationCO(int column, int row)
    {
        //Вычисляем позицию бонуса и партиклов
        dotPosition = new Vector2(column, row);
        Vector2 particleDownPosition = new Vector2(column, row - .5f);
        Vector2 particleUpPosition = new Vector2(column, row + .5f);

        //Создание секкввенции
        Sequence sequence = DOTween.Sequence();

        //Иницилизируем объекты
        GameObject rocketUp = Instantiate(columnRocketUp, dotPosition, Quaternion.identity);
        GameObject rocketDown = Instantiate(columnRocketDown, dotPosition, Quaternion.identity);

        //Анимация
        for (int i = 0; i < board.Height; i++)
        {
            sequence.Join(rocketUp.transform.DOMove(new Vector2(column, row + i + 1), 1))
                .Join(rocketDown.transform.DOMove(new Vector2(column, row - i - 1), 1));

            if (column >= 0 && column < board.Height)
            {
                if (board.allDots[column, i] != null)
                {
                    if (board.allDots[column, i].GetComponent<Dot>().isRowBomb)
                    {
                        continue;
                    }
                    if (board.allDots[column, i].GetComponent<Dot>().isColorBomb)
                    {
                        continue;
                    }
                    if (board.allDots[column, i].GetComponent<Dot>().isAdjacentBomb)
                    {
                        continue;
                    }
                    else
                    {
                        board.allDots[column, i].GetComponent<Dot>().isMatched = true;
                    }
                }
            }
        }

        sequence.Join(rocketUp.transform.DOMove(new Vector2(column, row + 20 + 1), 3))
            .Join(rocketDown.transform.DOMove(new Vector2(column, row - 20 - 1), 3));

        yield return new WaitForSeconds(6f);

        //Уначтожение объектов
        Destroy(rocketUp);
        Destroy(rocketDown);
    }

    private IEnumerator RowBombAnimationCO(int column, int row)
    {
        //Вычисляем позицию бонуса и партиклов
        dotPosition = new Vector2(column, row);
        Vector2 particleLeftPosition = new Vector2(column - .5f, row);
        Vector2 particleRightPosition = new Vector2(column + .5f, row);

        //Создание секкввенции
        Sequence sequence = DOTween.Sequence();

        //Иницилизируем объекты
        GameObject rocketRight = Instantiate(rowRocketRight, dotPosition, Quaternion.identity);
        GameObject rocketLeft = Instantiate(rowRocketLeft, dotPosition, Quaternion.identity);

        //Анимация
        for (int i = 0; i < board.Width; i++)
        {
            sequence.Join(rocketRight.transform.DOMove(new Vector2(column + i + 1, row), 1))
                .Join(rocketLeft.transform.DOMove(new Vector2(column - i - 1, row), 1));

            if (row >= 0 && row < board.Width)
            {
                if (board.allDots[i, row] != null)
                {
                    if (board.allDots[i, row].GetComponent<Dot>().isColumnBomb)
                    {
                        continue;
                    }
                    if (board.allDots[i, row].GetComponent<Dot>().isColorBomb)
                    {
                        continue;
                    }
                    if (board.allDots[i, row].GetComponent<Dot>().isAdjacentBomb)
                    {
                        continue;
                    }
                    else
                    {
                        board.allDots[i, row].GetComponent<Dot>().isMatched = true;
                    }
                }
            }
        }

        sequence.Join(rocketRight.transform.DOMove(new Vector2(column + 20 + 1, row), 3))
            .Join(rocketLeft.transform.DOMove(new Vector2(column - 20 - 1, row), 3));
        
        yield return new WaitForSeconds(6f);

        //Уначтожение объектов
        Destroy(rocketRight);
        Destroy(rocketLeft);
    }

    private IEnumerator ColumnRowBombAnimationCO(int column, int row)
    {
        yield return null;

        ColumnBombAnimation(column, row);
        RowBombAnimation(column, row);
    }

    private IEnumerator MegaColumnBombAnimationCO(int column, int row)
    {
        yield return null;

        ColumnBombAnimation(column - 1, row);
        ColumnBombAnimation(column, row);
        ColumnBombAnimation(column + 1, row);
    }

    //TODO: написать отдельную анимацию
    private IEnumerator MegaRowBombAnimationCO(int column, int row)
    {
        yield return null;

        RowBombAnimation(column, row - 1);
        RowBombAnimation(column, row);
        RowBombAnimation(column, row + 1);
    }

    private IEnumerator AdjacentBombAnimationCO(int column, int row)
    {
        //Вычисляем позицию бонуса
        dotPosition = new Vector2(column, row);

        Debug.Log("Adjacent Bomb Animation");

        //TODO: создать игровой элемент, у которого булет иде анимация взрыва

        yield return null;
    }
}
