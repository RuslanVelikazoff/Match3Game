using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [Header("Переменные для доски")]
    public int column;
    public int row;
    public int targetX;
    public int targetY;

    [Header("Переменные для свайпа")]
    public float SwipeAngle = 0;
    public float SwipeResist = 1f;

    [Header("Бонусы")]
    public bool isColorBomb = false;
    public bool isColumnBomb = false;
    public bool isRowBomb = false;
    public bool isAdjacentBomb = false;
    public GameObject colorBomb;

    [Header("Предыдущие значения")]
    public int previousColumn;
    public int previousRow;
    public GameObject otherDot;

    [Header("Уничтоженна ли точка")]
    public bool isMatched = false;

    [Header("Значения для перемещения")]
    private Vector2 firstTouchPosition = Vector2.zero;
    private Vector2 finalTouchPosition = Vector2.zero;
    private Vector2 tempPosition;

    [Header("Анимации")]
    private Animator animator;
    private float shineDelay;
    private float shineDelaySeconds;

    [Header("Вспомогательные объекты")]
    private BonusDot Bonus;
    private DotAnimation dotAnimation;
    private EndGameManager endGameManager;
    private FindMatches findMatches;
    private Board board;

    [Header("Костыль")]
    [HideInInspector]public bool otherBombActive = true;
    private float LastTimeClick = 0.0f;

    private void Start()
    {
        shineDelay = Random.Range(3f, 6f);
        shineDelaySeconds = shineDelay;

        Bonus = FindObjectOfType<BonusDot>();
        dotAnimation = FindObjectOfType<DotAnimation>();
        animator = GetComponent<Animator>();
        board = GameObject.FindWithTag("Board").GetComponent<Board>(); 
        findMatches = FindObjectOfType<FindMatches>();
        endGameManager = FindObjectOfType<EndGameManager>();
    }

    //Это для тестов и выводов консоль
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MakeAdjacentBomb();
        }
        if (Input.GetMouseButton(2))
        {
            MakeColumnBomb();
        }
    }

    #region DoubleClick
    void ActivateBonusDoubleClick()
    {
        StartCoroutine(ActivateBonusDoubleClickCO());
    }

    IEnumerator ActivateBonusDoubleClickCO()
    {
        yield return new WaitForSeconds(.3f);
        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                if (board.allDots[i, j] != null)
                {
                    if (board.allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        board.DestroyMatchesAt(i, j);
                    }
                }
            }
        }
        board.DecreaseRow();
    }
    #endregion

    private void Update()
    {
        if (tag == "Adjacent Bomb" || tag == "Color" || tag == "Column Bomb" || tag == "Row Bomb")
        {
            if (Input.GetMouseButtonDown(0))
            {
                float timeFromLastClick = Time.time - LastTimeClick;
                LastTimeClick = Time.time;
                if (timeFromLastClick < .2f)
                {
                    if (tag == "Adjacent Bomb")
                    {
                        Bonus.AdjacentBomb(column, row);
                        ActivateBonusDoubleClick();
                    }
                    if (tag == "Color")
                    {
                        Bonus.NeardyColorBomb(column, row);
                        ActivateBonusDoubleClick();
                    }
                    if (tag == "Column Bomb")
                    {
                        Bonus.ColumnBomb(column, row);
                        ActivateBonusDoubleClick();

                    }
                    if (tag == "Row Bomb")
                    {
                        Bonus.RowBomb(row, column);
                        ActivateBonusDoubleClick();
                    }
                }
            }
        }

        shineDelaySeconds -= Time.deltaTime;
        if (shineDelaySeconds <= 0)
        {
            shineDelaySeconds = shineDelay;
            //StartCoroutine(StartShineCo());
        }

        targetX = column;
        targetY = row;

        //Свайпы влево и вправо
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
                findMatches.FindAllMatches();//Поиск совпадений
            }
        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }

        //Свайпы вверх и вниз
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
                findMatches.FindAllMatches();//Поиск совпадений
            }
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }

    private void OnMouseDown()
    {
        if (animator != null)
        {
            animator.SetBool("Touched", true);
        }
        
        if (board.currentState == GameState.move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp()
    {
        animator.SetBool("Touched", false);
        if (board.currentState == GameState.move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    /*IEnumerator StartShineCo()
    {
        animator.SetBool("Shine", true);
        yield return null;
        animator.SetBool("Shine", false);
    }*/

    public void PopAnimation()
    {
        //animator.SetBool("Popped", true);
        //TODO: включить вибрацию
        //Handheld.Vibrate();
    }

    public IEnumerator CheckMoveCo()
    {
        //Переделываем игровые объекты в точки
        Dot otherDota = otherDot.GetComponent<Dot>();
        Dot currentDota = this.gameObject.GetComponent<Dot>();

        #region Цветная бомба
        //Проверка на цветную бомбу
        if (isColorBomb && otherDota.isColorBomb)
        {
            otherBombActive = false;
            Bonus.MegaColorBomb();
            otherBombActive = true;
        }
        else if (isColorBomb && otherDota.isAdjacentBomb)
        {
            otherBombActive = false;
            Bonus.ComboColorAdjacent(currentDota, otherDota);
            otherBombActive = true;
        }
        else if (isColorBomb && otherDota.isColumnBomb)
        {
            otherBombActive = false;
            Bonus.ComboColorColumn(currentDota, otherDota);
            otherBombActive = true;
        }
        else if (isColorBomb && otherDota.isRowBomb)
        {
            otherBombActive = false;
            Bonus.ComboColorRow(currentDota, otherDota);
            otherBombActive = true;
        }
        #endregion

        #region Квадратная бомба
        //Проверка на квадратную бомбу
        else if (isAdjacentBomb && otherDota.isAdjacentBomb)
        {
            otherBombActive = false;
            otherDota.isAdjacentBomb = false;
            otherDota.isMatched = true;
            isMatched = true;
            Bonus.MegaAdjacentBomb(currentDota.column, currentDota.row);
            otherBombActive = true;
        }
        else if (isAdjacentBomb && otherDota.isColorBomb)
        {
            otherBombActive = false;
            Bonus.ComboColorAdjacent(currentDota, otherDota);
            otherBombActive = true;
        }
        else if (isAdjacentBomb && otherDot.GetComponent<Dot>().isColumnBomb)
        {
            otherBombActive = false;
            otherDota.isColumnBomb = false;
            otherDota.isMatched = true;
            isMatched = true;
            Bonus.MegaColumnAdjacentBomb(currentDota.column, currentDota.row);
            otherBombActive = true;
        }
        else if (isAdjacentBomb && otherDota.isRowBomb)
        {
            otherBombActive = false;
            otherDota.isRowBomb = false;
            otherDota.isMatched = true;
            isMatched = true;
            Bonus.MegaRowAdjacentBomb(currentDota.column, currentDota.row);
            otherBombActive = true;
        }
        #endregion

        #region Столбцовая бомба
        //Проверка на столбцовую бомбу
        else if (isColumnBomb && otherDota.isColumnBomb)
        {
            otherBombActive = false;
            otherDota.isColumnBomb = false;
            isColumnBomb = false;
            otherDota.isMatched = true;
            isMatched = true;
            Bonus.MegaColumnRowBomb(currentDota.column, currentDota.row);
            otherBombActive = true;
        }
        else if (isColumnBomb && otherDot.GetComponent<Dot>().isColorBomb)
        {
            otherBombActive = false;
            Bonus.ComboColorColumn(currentDota, otherDota);
            otherBombActive = true;
        }
        else if (isColumnBomb && otherDota.isAdjacentBomb)
        {
            otherBombActive = false;
            otherDota.isAdjacentBomb = false;
            otherDota.isMatched = true;
            isMatched = true;
            Bonus.MegaColumnAdjacentBomb(currentDota.column, currentDota.row);
            otherBombActive = true;
        }
        else if (isColumnBomb && otherDota.isRowBomb)
        {
            otherBombActive = false;
            otherDota.isRowBomb = false;
            isColumnBomb = false;
            otherDota.isMatched = true;
            isMatched = true;
            Bonus.MegaColumnRowBomb(currentDota.column, currentDota.row);
            otherBombActive = true;
        }
        #endregion

        #region Рядовая бомба
        else if (isRowBomb && otherDota.isRowBomb)
        {
            otherBombActive = false;
            otherDota.isRowBomb = false;
            isRowBomb = false;
            otherDota.isMatched = true;
            isMatched = true;
            Bonus.MegaColumnRowBomb(currentDota.column, currentDota.row);
            otherBombActive = true;
        }
        else if (isRowBomb && otherDota.isColorBomb)
        {
            otherBombActive = false;
            Bonus.ComboColorRow(currentDota, otherDota);
            otherBombActive = true;
        }
        else if (isRowBomb && otherDota.isAdjacentBomb)
        {
            otherBombActive = false;
            otherDota.isAdjacentBomb = false;
            otherDota.isMatched = true;
            isMatched = true;
            Bonus.MegaRowAdjacentBomb(currentDota.column, currentDota.row);
            otherBombActive = true;
        }
        else if (isRowBomb && otherDota.isColumnBomb)
        {
            otherBombActive = false;
            otherDota.isColumnBomb = false;
            otherDota.isMatched = true;
            isMatched = true;
            Bonus.MegaColumnRowBomb(currentDota.column, currentDota.row);
            otherBombActive = true;
        }
        #endregion

        else
        {
            //Цветная бомба
            if (isColorBomb && otherBombActive)
            {
                //Эта точка - цветная бомба
                Bonus.MatchPiecesOfColor(otherDot.tag);
                isMatched = true;
            }
            else if (otherDota.isColorBomb && otherBombActive)
            {
                //Другая точка - цветная бомба
                Bonus.MatchPiecesOfColor(this.gameObject.tag);
                otherDota.isMatched = true;
            }

            //Квадратная бомба
            if (isAdjacentBomb && otherBombActive)
            {
                //Эта точка - соседняя бомба
                Bonus.AdjacentBomb(currentDota.column, currentDota.row);
                isMatched = true;
            }
            else if (otherDota.isAdjacentBomb && otherBombActive)
            {
                //Другая точкка - соседняя бомба
                Bonus.AdjacentBomb(otherDota.column, otherDota.row);
                otherDota.isMatched = true;
            }

            //Столбцовая бомба
            if (isColumnBomb && otherBombActive)
            {
                //Эта точка - столбцовая бомба
                Bonus.ColumnBomb(currentDota.column, currentDota.row);
                if (otherDota.column != currentDota.column)
                {
                    otherDota.isMatched = false;
                }
                isMatched = true;
            }
            else if (otherDota.isColumnBomb && otherBombActive)
            {
                //Другая точка - столбцовая бомба
                Bonus.ColumnBomb(otherDota.column, otherDota.row);
                if (currentDota.column != otherDota.column)
                {
                    isMatched = false;
                }
                otherDota.isMatched = true;
            }

            //Рядовая бомба
            if (isRowBomb && otherBombActive)
            {
                //Эта точка - рядовая бомба
                Bonus.RowBomb(currentDota.row, currentDota.column);
                if (otherDota.row != currentDota.row)
                {
                    otherDota.isMatched = false;
                }
                isMatched = true;
            }
            else if (otherDota.isRowBomb && otherBombActive)
            {
                //Другая точка рядовая бомба
                Bonus.RowBomb(otherDota.row, otherDota.column);
                if (currentDota.row != otherDota.row)
                {
                    isMatched = false;
                }
                otherDota.isMatched = true;
            }
        }

        //Если не находит совпаденний, то возвращает плитку обратно
        yield return new WaitForSeconds(.5f);
        if (otherDot != null)
        {
            if (!isMatched && !otherDota.isMatched)
            {
                otherDota.row = row;
                otherDota.column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(.5f);
                board.currentDot = null;
                board.currentState = GameState.move;
            }
            else
            {
                if(endGameManager != null)
                {
                    if (endGameManager.requirements.gameType == GameType.Moves)
                    {
                        endGameManager.DecreaseCounerValue();
                    }
                }

                board.DestroyMatches();
            }
        }

        yield return new WaitForSeconds(.2f);

        findMatches.FindAllMatches();
    }

    void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > SwipeResist
            || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > SwipeResist)
        {
            board.currentState = GameState.wait;
            SwipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
            board.currentDot = this;
        }
        else
        {
            board.currentState = GameState.move;
        }
    }

    void MovePiecesActual(Vector2 direction)
    {
        otherDot = board.allDots[column + (int) direction.x, row + (int) direction.y];
        previousRow = row;
        previousColumn = column;

        if (otherDot != null)
        {
            otherDot.GetComponent<Dot>().column += -1 * (int) direction.x;
            otherDot.GetComponent<Dot>().row += -1 * (int) direction.y;
            column += (int) direction.x;
            row += (int) direction.y;
            StartCoroutine(CheckMoveCo());
        }
        else
        {
            board.currentState = GameState.move;
        }
    }

    void MovePieces()
    {
        if (SwipeAngle > -45 && SwipeAngle <= 45 && column < board.Width - 1)
        {
            //Свайп вправо
            MovePiecesActual(Vector2.right);
        }

        else if (SwipeAngle > 45 && SwipeAngle <= 135 && row < board.Height - 1)
        {
            //Свайп вверх
            MovePiecesActual(Vector2.up);
        }

        else if ((SwipeAngle > 135 || SwipeAngle <= -135) && column > 0)
        {
            //Свайп влево
            MovePiecesActual(Vector2.left);
        }

        else if (SwipeAngle < -45 && SwipeAngle >= -135 && row > 0)
        {
            //Свайп вниз
            MovePiecesActual(Vector2.down);
        }
        else
        {
            board.currentState = GameState.move;
        }
    }

    void FindMatches()
    {
        if (column > 0 && column < board.Width - 1)
        {
            GameObject leftDot1 = board.allDots[column - 1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];

            if (leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }

        if (row > 0 && row < board.Height - 1)
        {
            GameObject upDot1 = board.allDots[column, row + 1];
            GameObject downDot1 = board.allDots[column, row - 1];
            if (upDot1 != null && downDot1 != null)
            {
                if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
                {
                    upDot1.GetComponent<Dot>().isMatched = true;
                    downDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }

    public void MakeRowBomb()
    {
        if (!isColumnBomb && !isColorBomb && !isAdjacentBomb)
        {
            animator.SetBool("isRow", true);
            isRowBomb = true;
            this.gameObject.tag = "Row Bomb";
        }
    }

    public void MakeColumnBomb()
    {
        if (!isRowBomb && !isColorBomb && !isAdjacentBomb)
        {
            animator.SetBool("isColumn", true);
            isColumnBomb = true;
            this.gameObject.tag = "Column Bomb";
        }
    }

    public void MakeColorBomb()
    {
        if (!isColumnBomb && !isRowBomb && !isAdjacentBomb)
        {
            isColorBomb = true;
            GameObject color = Instantiate(colorBomb, transform.position, Quaternion.identity);
            color.transform.parent = this.transform;
            this.gameObject.tag = "Color";
        }
    }

    public void MakeAdjacentBomb()
    {
        if (!isRowBomb && !isColumnBomb && !isColorBomb)
        {
            animator.SetBool("isAdjacent", true);
            isAdjacentBomb = true;
            this.gameObject.tag = "Adjacent Bomb";
        }
    }
}

