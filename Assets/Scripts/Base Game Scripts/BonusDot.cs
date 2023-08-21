using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BonusDot : MonoBehaviour
{
    private Board board;
    private DotAnimation dotAnimation;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        dotAnimation = FindObjectOfType<DotAnimation>();
    }

    public void MatchPiecesOfColor(string color)
    {
        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                //Проверить существует ли эта часть
                if (board.allDots[i, j] != null)
                {
                    //Проверка тэга этой точки
                    if (board.allDots[i, j].tag == color)
                    {
                        //Установить эту точку для сопостовления
                        board.allDots[i, j].GetComponent<Dot>().isMatched = true;
                    }
                }
            }
        }
    }

    //TODO: добавить активацию соседней бомбы, после добавления анимации
    public void NeardyColorBomb(int column, int row)
    {
        //Генерируем рандомную точку и инициализируем её
        int randomDot = Random.Range(0, board.Dots.Length);
        Dot dot = board.Dots[randomDot].GetComponent<Dot>();

        MatchPiecesOfColor(dot.tag);

        Dot colorBomb = board.allDots[column, row].GetComponent<Dot>();
        colorBomb.isMatched = true;
    }

    public void NeardyAdjacentBomb(int oldColumn, int oldRow)
    {
        for (int x = oldColumn - 1; x <= oldColumn + 1; x++)
        {
            for (int y = oldRow - 1; y <= oldRow + 1; y++)
            {
                if (x >= 0 && x < board.Width && y >= 0 && y < board.Height)
                {
                    if (board.allDots[x, y] != null)
                    {
                        board.allDots[x, y].GetComponent<Dot>().isMatched = true;
                    }
                }
            }
        }
    }

    public void AdjacentBomb(int column, int row)
    {
        dotAnimation.AdjacentBombAnimation(column, row);
        for (int i = column - 1; i <= column + 1; i++)
        {
            for (int j = row - 1; j <= row + 1; j++)
            {
                if (i >= 0 && i < board.Width && j >= 0 && j < board.Height)
                {
                    if (board.allDots[i, j] != null)
                    {
                        Dot dot = board.allDots[i, j].GetComponent<Dot>();
                        dot.isMatched = true;
                    }
                }
            }
        }
    }

    #region ColumnBomb
    public void ColumnBomb(int column, int row)
    {
        StartCoroutine(ColumnBombCO(column, row));
    }

    private IEnumerator ColumnBombCO(int column, int row)
    {
        dotAnimation.ColumnBombAnimation(column, row);

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < board.Height; i++)
        {
            if (board.allDots[column, i] != null)
            {
                if (board.allDots[column, i].GetComponent<Dot>().isRowBomb)
                {
                    dotAnimation.RowBombAnimation(column, i);
                }
                if (board.allDots[column, i].GetComponent<Dot>().isColorBomb)
                {
                    NeardyColorBomb(column, i);
                }
            }
        }
    }
    #endregion

    #region RowBomb
    public void RowBomb(int row, int column)
    {
        StartCoroutine(RowBombCO(row, column));
    }

    private IEnumerator RowBombCO(int row, int column)
    {
        dotAnimation.RowBombAnimation(column, row);

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < board.Width; i++)
        {
            if (board.allDots[i, row] != null)
            {
                if (board.allDots[i, row].GetComponent<Dot>().isColumnBomb)
                {
                    dotAnimation.ColumnBombAnimation(i, row);
                }
                if (board.allDots[i, row].GetComponent<Dot>().isColorBomb)
                {
                    NeardyColorBomb(i, row);
                }
            }
        }
    }
    #endregion

    public void MegaAdjacentBomb(int column, int row)
    {
        for (int i = column - 2; i <= column + 2; i++)
        {
            for (int j = row - 2; j <= row + 2; j++)
            {
                if (i >= 0 && i < board.Width && j >= 0 && j < board.Height)
                {
                    if (board.allDots[i, j] != null)
                    {
                        Dot dot = board.allDots[i, j].GetComponent<Dot>();
                        dot.isMatched = true;
                        
                    }
                }
            }
        }
    }

    #region ColumnRowBomb
    public void MegaColumnRowBomb(int column, int row)
    {
        StartCoroutine(MegaColumnRowBombCO(column, row));
    }

    private IEnumerator MegaColumnRowBombCO(int column, int row)
    {
        dotAnimation.ColumnRowBombAnimation(column, row);

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < board.Width; i++)
        {
            if (board.allDots[column, i] != null)
            {
                if (board.allDots[column, i].GetComponent<Dot>().isRowBomb)
                {
                    dotAnimation.RowBombAnimation(column, i);
                }
                if (board.allDots[column, i].GetComponent<Dot>().isColorBomb)
                {
                    NeardyColorBomb(column, i);
                }
            }
        }

        for (int i = 0; i < board.Height; i++)
        {
            if (board.allDots[i, row] != null)
            {
                if (board.allDots[i, row].GetComponent<Dot>().isColumnBomb)
                {
                    dotAnimation.ColumnBombAnimation(i, row);
                }
                if (board.allDots[i, row].GetComponent<Dot>().isColorBomb)
                {
                    NeardyColorBomb(i, row);
                }
            }
        }
    }
#endregion

    public void MegaColorBomb()
    {
        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                if (board.allDots[i, j] != null)
                {
                    board.allDots[i, j].GetComponent<Dot>().isMatched = true;
                }
            }
        }
    }

    #region MegaColumnBomb
    public void MegaColumnAdjacentBomb(int column, int row)
    {
        StartCoroutine(MegaColumnAdjacentBombCO(column, row));
    }

    private IEnumerator MegaColumnAdjacentBombCO(int column, int row)
    {
        dotAnimation.MegaColumnBombAnimation(column, row);

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < board.Height; i++)
        {
            if (board.allDots[column, i] != null)
            {
                if (board.allDots[column, i].GetComponent<Dot>().isRowBomb)
                {
                    dotAnimation.RowBombAnimation(column, i);
                }
                if (board.allDots[column, i].GetComponent<Dot>().isColorBomb)
                {
                    NeardyColorBomb(column, row);
                }
            }

            if (column - 1 >= 0)
            {
                if (board.allDots[column - 1, i] != null)
                {
                    if (board.allDots[column - 1, i].GetComponent<Dot>().isRowBomb)
                    {
                        dotAnimation.RowBombAnimation(column - 1, i);
                    }
                    if (board.allDots[column - 1, i].GetComponent<Dot>().isColorBomb)
                    {
                        NeardyColorBomb(column - 1, i);
                    }
                }
            }

            if (column + 1 < board.Height)
            {
                if (board.allDots[column + 1, i] != null)
                {
                    if (board.allDots[column + 1, i].GetComponent<Dot>().isRowBomb)
                    {
                        dotAnimation.RowBombAnimation(column + 1, i);
                    }
                    if (board.allDots[column + 1, i].GetComponent<Dot>().isColorBomb)
                    {
                        NeardyColorBomb(column + 1, i);
                    }
                }
            }
        }
    }
    #endregion

    #region MegaRowBomb
    public void MegaRowAdjacentBomb(int column, int row)
    {
        StartCoroutine(MegaRowAdjacentBombCO(column, row));
    }

    private IEnumerator MegaRowAdjacentBombCO(int column, int row)
    {
        dotAnimation.MegaRowBombAnimation(column, row);

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < board.Width; i++)
        {
            if (board.allDots[i, row] != null)
            {
                if (board.allDots[i, row].GetComponent<Dot>().isColumnBomb)
                {
                    dotAnimation.ColumnBombAnimation(i, row);
                }
                if (board.allDots[i, row].GetComponent<Dot>().isColorBomb)
                {
                    NeardyColorBomb(column, row);
                }
            }

            if (row - 1 >= 0)
            {
                if (board.allDots[i, row - 1] != null)
                {
                    if (board.allDots[i, row - 1].GetComponent<Dot>().isColumnBomb)
                    {
                        dotAnimation.ColumnBombAnimation(i, row - 1);
                    }
                    if (board.allDots[i, row - 1].GetComponent<Dot>().isColorBomb)
                    {
                        NeardyColorBomb(i, row - 1);
                    }
                }
            }

            if (row + 1 < board.Width)
            {
                if (board.allDots[i, row + 1] != null)
                {
                    if (board.allDots[i, row + 1].GetComponent<Dot>().isColumnBomb)
                    {
                        dotAnimation.ColumnBombAnimation(i, row + 1);
                    }
                    if (board.allDots[i, row + 1].GetComponent<Dot>().isColorBomb)
                    {
                        NeardyColorBomb(i, row + 1);
                    }
                }
            }
        }
    }
    #endregion

    //Color bomb
    public void ComboColorRow(Dot currentDot, Dot otherDot)
    {
        int randomColor = Random.Range(0, board.Dots.Length);
        Dot RandomDot = board.Dots[randomColor].GetComponent<Dot>();

        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                if (board.allDots[i, j] != null)
                {
                    if (board.allDots[i, j].tag == RandomDot.tag)
                    {
                        Dot dot = board.allDots[i, j].GetComponent<Dot>();
                        dot.MakeRowBomb();
                        RowBomb(dot.row, dot.column);
                    }
                }
            }
        }

        currentDot.isMatched = true;
        otherDot.isMatched = true;
    }

    public void ComboColorColumn(Dot currentDot, Dot otherDot)
    {
        int randomColor = Random.Range(0, board.Dots.Length);
        Dot RandomDot = board.Dots[randomColor].GetComponent<Dot>();

        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                if (board.allDots[i, j] != null)
                {
                    if (board.allDots[i, j].tag == RandomDot.tag)
                    {
                        Dot dot = board.allDots[i, j].GetComponent<Dot>();
                        dot.MakeColumnBomb();
                        ColumnBomb(dot.column, dot.row);
                    }
                }
            }
        }

        currentDot.isMatched = true;
        otherDot.isMatched = true;
    }

    public void ComboColorAdjacent(Dot currentDot, Dot otherDot)
    {
        int randomColor = Random.Range(0, board.Dots.Length);
        Dot RandomDot = board.Dots[randomColor].GetComponent<Dot>();

        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                if (board.allDots[i, j] != null)
                {
                    if (board.allDots[i, j].tag == RandomDot.tag)
                    {
                        Dot dot = board.allDots[i, j].GetComponent<Dot>();
                        dot.MakeAdjacentBomb();
                        AdjacentBomb(dot.column, dot.row);
                    }
                }
            }
        }

        currentDot.isMatched = true;
        otherDot.isMatched = true;
    }
}
