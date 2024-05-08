using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait,
    move,
    win,
    lose,
    pause
}

public enum TileKind
{
    Concrete,
    Normal
}

public enum PieceKind
{
    ColorBomb,
    RowBomb,
    ColumnBomb,
    AdjacentBomb,
    Normal
}

[System.Serializable]
public class MatchType
{
    public int type;
    public string color;
}

[System.Serializable]
public class TileType
{
    public int x;
    public int y;

    public TileKind tileKind;

}

[System.Serializable]
public class PieceType
{
    public int x;
    public int y;

    public PieceKind pieceKind;
}

public class Board : MonoBehaviour
{
    [Header("Настройка уровня")]
    public World world;
    public int level;
    public GameState currentState = GameState.move;
    public float refilDelay = 0.5f;
    public float fallDelay = 0.2f;

    [Header("Размер поля")]
    public int Width;
    public int Height;
    public int offSet;

    [Header("Настройка уровня")]
    public TileType[] boardLayout;
    public PieceType[] pieceLayout;
    public GameObject[] Dots; //Игровые элементы

    [Header("Префабы")]
    public GameObject tilePrefab; //размер плитки 512 х 512
    public GameObject destroyEffect;
    public GameObject concreteTilePrefab;
    private bool[,] blankSpaces; //Игровое поле
    public BackgroundTile[,] concreteTiles;
    public GameObject[,] allDots;
    public Dot currentDot;

    [Header("Вспомогательные объекты")]
    public MatchType matchType;
    private FindMatches findMatches;
    private GoalManager goalManager;

    private void Awake()
    {
        if(PlayerPrefs.HasKey("Current Level"))
        {
            level = PlayerPrefs.GetInt("Current Level");
        }
        if (world != null)
        {
            if (level < world.levels.Length)
            {
                if (world.levels[level] != null)
                {
                    Width = world.levels[level].width;
                    Height = world.levels[level].height;
                    Dots = world.levels[level].dots;
                    boardLayout = world.levels[level].boardLayout;
                    pieceLayout = world.levels[level].pieceLayout;
                }
            }
        }
    }

    private void Start()
    {
        blankSpaces = new bool[Width, Height]; //Инициализация поля
        allDots = new GameObject[Width, Height]; //Инициализация элементов
        concreteTiles = new BackgroundTile[Width, Height]; //Инициализация бетонных элементов

        findMatches = FindObjectOfType<FindMatches>();
        goalManager = FindObjectOfType<GoalManager>();

        SetUp();

        currentState = GameState.pause;
    }

    private void Update()
    {
        findMatches.FindAllMatches();
    }

    public void GenerateConcreteTiles()
    {
        //Просмотр всего поля
        for (int i = 0; i < boardLayout.Length; i++)
        {
            //Если плитка является Закрытой
            if (boardLayout[i].tileKind == TileKind.Concrete)
            {
                //Создать бетонную плитку на этом месте
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(concreteTilePrefab, tempPosition, Quaternion.identity);

                concreteTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }

    public void GenerateColorBomb()
    {
        for (int i = 0; i < pieceLayout.Length; i++)
        {
            if (pieceLayout[i].pieceKind == PieceKind.ColorBomb)
            {
                allDots[pieceLayout[i].x, pieceLayout[i].y].GetComponent<Dot>().MakeColorBomb();
            }
        }
    }

    public void GenerateRowBomb()
    {
        for (int i = 0; i < pieceLayout.Length; i++)
        {
            if (pieceLayout[i].pieceKind == PieceKind.RowBomb)
            {
                allDots[pieceLayout[i].x, pieceLayout[i].y].GetComponent<Dot>().MakeRowBomb();
            }
        }
    }

    public void GenerateColumnBomb()
    {
        for (int i = 0; i < pieceLayout.Length; i++)
        {
            if (pieceLayout[i].pieceKind == PieceKind.ColumnBomb)
            {
                allDots[pieceLayout[i].x, pieceLayout[i].y].GetComponent<Dot>().MakeColumnBomb();
            }
        }
    }

    public void GenerateAdjacentBomb()
    {
        for (int i = 0; i < pieceLayout.Length; i++)
        {
            if (pieceLayout[i].pieceKind == PieceKind.AdjacentBomb)
            {
                allDots[pieceLayout[i].x, pieceLayout[i].y].GetComponent<Dot>().MakeAdjacentBomb();
            }
        }
    }

    private void SetUp()  
    {
        GenerateConcreteTiles();

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (!blankSpaces[i, j] && !concreteTiles[i, j])
                {
                    //Создание поля
                    Vector2 temoPosition = new Vector2(i, j + offSet);
                    Vector2 tilePosition = new Vector2(i, j);
                    GameObject backgorundTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                    backgorundTile.transform.parent = this.transform;
                    backgorundTile.name = "( " + i + ", " + j + " )";

                    //Создание элементов
                    int DotToUse = Random.Range(0, Dots.Length);

                    //Проверка совпадений
                    int maxIterations = 0;
                    while (MatchesAt(i, j, Dots[DotToUse]) && maxIterations < 100)
                    {
                        DotToUse = Random.Range(0, Dots.Length);
                        maxIterations++;
                    }
                    maxIterations = 0;

                    GameObject Dot = Instantiate(Dots[DotToUse], temoPosition, Quaternion.identity);
                    Dot.GetComponent<Dot>().row = j;
                    Dot.GetComponent<Dot>().column = i;
                    Dot.transform.parent = this.transform;
                    Dot.name = "Dot( " + i + ", " + j + " )";
                    allDots[i, j] = Dot;
                }
            }
        }

        GenerateColorBomb();
        GenerateRowBomb();
        GenerateColumnBomb();
        GenerateAdjacentBomb();
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }

            if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
            {
                if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
                {
                    if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }

            if (column > 1)
            {
                if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
                {
                    if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public MatchType ColumnOrRow()
    {
        //Создаем копию текущих совпадений
        List<GameObject> matchCopy = findMatches.currentMatches as List<GameObject>;

        matchType.type = 0;
        matchType.color = "";

        //Пройтись по всему списку совпадений и решить нужно ли делать бомбу
        for (int i = 0; i < matchCopy.Count; i++)
        {
            //Сохраняем эту точку
            Dot thisDot = matchCopy[i].GetComponent<Dot>();

            string color = matchCopy[i].tag;

            int column = thisDot.column;
            int row = thisDot.row;
            int columnMatch = 0;
            int rowMatch = 0;

            //Перебираем остальные компоненты и сравниваем
            for (int j = 0; j < matchCopy.Count; j++)
            {
                //Сохраняем следующую точку
                Dot nextDot = matchCopy[j].GetComponent<Dot>();

                if (nextDot == thisDot)
                {
                    continue;
                }

                if (nextDot.column == thisDot.column && nextDot.tag == color)
                {
                    columnMatch++;
                    Debug.Log("ColumnMatch: " + columnMatch);
                }
                if (nextDot.row == thisDot.row && nextDot.tag == color)
                {
                    rowMatch++;
                    Debug.Log("RowMatch: " + rowMatch);
                }
            }

            //Вернуть 3, если столбик или строка совпала
            //Вернуть 2, если это соседняя бомба
            //Вернуть 1, если это цветная бомба
            if (columnMatch == 4 || rowMatch == 4)
            {
                matchType.type = 1;
                matchType.color = color;
                return matchType;
            }
            else if (columnMatch == 2 && rowMatch == 2)
            {
                matchType.type = 2;
                matchType.color = color;
                return matchType;
            }
            else if (columnMatch == 3 && rowMatch == 2 || columnMatch == 2 && rowMatch == 3)
            {
                Debug.Log("Adjacent Bomb");
                matchType.type = 2;
                matchType.color = color;
                return matchType;
            }
            else if (columnMatch == 3 && rowMatch == 0 || rowMatch == 3 && columnMatch == 0)
            {
                Debug.Log("Column: " + columnMatch);
                Debug.Log("Row: " + rowMatch);
                matchType.type = 3;
                matchType.color = color;
                return matchType;
            }
        }
        matchType.type = 0;
        matchType.color = "";
        return matchType;
    }

    public void CheckToMakeBombs()
    {
        //Сколько объэектов в findMatches.currentMatches
        if (findMatches.currentMatches.Count > 3)
        {
            //Какой тип совпадения?
            MatchType typeOfMatch = ColumnOrRow();

            if (typeOfMatch.type == 1)
            {
                //Создание цветной бомбы
                //Совпадает ли текушая точка?
                if (currentDot != null && currentDot.isMatched && currentDot.tag == typeOfMatch.color)
                {
                    currentDot.isMatched = false;
                    currentDot.MakeColorBomb();
                }
                else
                {
                    if (currentDot.otherDot != null)
                    {
                        Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                        if (otherDot.isMatched && otherDot.tag == typeOfMatch.color)
                        {
                            otherDot.isMatched = false;
                            otherDot.MakeColorBomb();
                        }
                    }
                }
            }

            else if (typeOfMatch.type == 2)
            {
                //Создание соседней бомбы
                //Совпадает ли текушая точка?
                if (currentDot != null && currentDot.isMatched && currentDot.tag == typeOfMatch.color)
                {
                    currentDot.isMatched = false;
                    currentDot.MakeAdjacentBomb();
                }
                else
                {
                    if (currentDot.otherDot != null)
                    {
                        Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                        if (otherDot.isMatched && otherDot.tag == typeOfMatch.color)
                        {
                            otherDot.isMatched = false;
                            otherDot.MakeAdjacentBomb();
                        }
                    }
                }
            }

            else if (typeOfMatch.type == 3)
            {
                findMatches.CheckBombs(typeOfMatch);
            }    
        }
    }

    public void BombRow(int row)
    {
        for (int i = 0; i < Width; i++)
        {
            if (concreteTiles[i, row])
            {
                concreteTiles[i, row].TakeDamage(1);
                if (concreteTiles[i, row].hitPoints <= 0)
                {
                    concreteTiles[i, row] = null;
                }
            }
        }
    }

    public void BombColumn(int column)
    {
        for (int i = 0; i < Width; i++)
        {
            if (concreteTiles[column, i])
            {
                concreteTiles[column, i].TakeDamage(1);
                if (concreteTiles[column, i].hitPoints <= 0)
                {
                    concreteTiles[column, i] = null;
                }
            }
        }
    }

    public void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {
            DamageConcrete(column, row);

            //Обновление панели с целями
            if (goalManager != null)
            {
                goalManager.CompareGoal(allDots[column, row].tag.ToString());
                goalManager.UpdateGoals();
            }

            RandomPopSound();

            //This is particles
            GameObject particle = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            Destroy(particle, .5f);

            allDots[column, row].GetComponent<Dot>().PopAnimation();

            Destroy(allDots[column, row], .5f);
            allDots[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        //Сколько элементов совпало?
        if (findMatches.currentMatches.Count >= 4)
        {
            CheckToMakeBombs();
        }
        findMatches.currentMatches.Clear();

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCo2());
    }

    private void DamageConcrete(int column, int row)
    {
        if (column > 0)
        {
            if (concreteTiles[column - 1, row])
            {
                concreteTiles[column - 1, row].TakeDamage(1);
                if (concreteTiles[column - 1, row].hitPoints <= 0)
                {
                    concreteTiles[column - 1, row] = null;
                    GameObject tile = Instantiate(tilePrefab, new Vector2(column - 1, row), Quaternion.identity);
                }
            }
        }

        if (column < Width - 1)
        {
            if (concreteTiles[column + 1, row])
            {
                concreteTiles[column + 1, row].TakeDamage(1);
                if (concreteTiles[column + 1, row].hitPoints <= 0)
                {
                    concreteTiles[column + 1, row] = null;
                    GameObject tile = Instantiate(tilePrefab, new Vector2(column + 1, row), Quaternion.identity);
                }
            }
        }

        if (row > 0)
        {
            if (concreteTiles[column, row - 1])
            {
                concreteTiles[column, row - 1].TakeDamage(1);
                if (concreteTiles[column, row - 1].hitPoints <= 0)
                {
                    concreteTiles[column, row - 1] = null;
                    GameObject tile = Instantiate(tilePrefab, new Vector2(column, row - 1), Quaternion.identity);
                }
            }
        }

        if (row < Height - 1)
        {
            if (concreteTiles[column, row + 1])
            {
                concreteTiles[column, row + 1].TakeDamage(1);
                if (concreteTiles[column, row + 1].hitPoints <= 0)
                {
                    concreteTiles[column, row + 1] = null;
                    GameObject tile = Instantiate(tilePrefab, new Vector2(column, row + 1), Quaternion.identity);
                }
            }
        }
    }

    public void DecreaseRow()
    {
        StartCoroutine(DecreaseRowCo2());
    }

    private IEnumerator DecreaseRowCo2()
    {
        yield return new WaitForSeconds(fallDelay);
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                //Если текущее место не пустое
                if (!blankSpaces[i, j] && allDots[i, j] == null && !concreteTiles[i, j])
                {
                    //Цикл от пробела выше до вершины столбца
                    for (int k = j + 1; k < Height; k++)
                    {
                        //Если точкa найдена
                        if (allDots[i, k] != null)
                        {
                            //Переместить эту точку в это пустое место
                            allDots[i, k].GetComponent<Dot>().row = j;

                            //Делаем это место null
                            allDots[i, k] = null;

                            //Прекращаем цикл
                            break;
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(refilDelay * 0.5f);
        StartCoroutine(FillBoardCo()); 
    }

    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (allDots[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    allDots[i, j] = null;

                }
            }
            nullCount = 0;
        }

        yield return new WaitForSeconds(refilDelay * 0.5f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (allDots[i, j] == null && !blankSpaces[i, j] && !concreteTiles[i, j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int dotToUse = Random.Range(0, Dots.Length);

                    int maxIterations = 0;
                    while (MatchesAt(i, j, Dots[dotToUse]) && maxIterations < 100)
                    {
                        maxIterations++;
                        dotToUse = Random.Range(0, Dots.Length);

                    }
                    maxIterations = 0;

                    GameObject piece = Instantiate(Dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        findMatches.FindAllMatches();
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private IEnumerator FillBoardCo()
    {
        yield return new WaitForSeconds(refilDelay);
        RefillBoard();
        yield return new WaitForSeconds(refilDelay);

        while (MatchesOnBoard())
        {
            DestroyMatches();
            yield break;
        }
        currentDot = null;

        if (isDeadlocker())
        {
            ShuffleBoard();
            Debug.Log("Тупик");
        }

        if (currentState != GameState.pause)
        {
            currentState = GameState.move;
        }
    }

    private Vector2 CheckForAdjacent(int column, int row)
    {
        if (column < Width - 1 && allDots[column + 1, row])
        {
            return Vector2.right;
        }
        if (column > 0 && allDots[column - 1, row])
        {
            return Vector2.left;
        }
        if (row < Height - 1 && allDots[column, row + 1])
        {
            return Vector2.up;
        }
        if (row > 0 && allDots[column, row - 1])
        {
            return Vector2.down;
        }
        return Vector2.zero;
    }

    private void SwitchPieces(int column, int row, Vector2 direction)
    {
        if (allDots[column + (int)direction.x, row + (int)direction.y] != null)
        {
            //Берем второй элемент и сохраняем его
            GameObject holder = allDots[column + (int)direction.x, row + (int)direction.y] as GameObject;

            //Переключаем первую точку на вторую позицию
            allDots[column + (int)direction.x, row + (int)direction.y] = allDots[column, row];

            //Установить первую точку, как вторуб точку
            allDots[column, row] = holder;
        }
    }

    private bool CheckForMatches()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (allDots[i, j] != null)
                {
                    //Убедиться, что первая и вторая справа находятся на доске
                    if (i < Width - 2)
                    {
                        //Проверка существуют ли точки вправо
                        if (allDots[i + 1, j] != null && allDots[i + 2, j] != null)
                        {
                            if (allDots[i + 1, j].tag == allDots[i, j].tag
                                && allDots[i + 2, j].tag == allDots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                    //Проверка, что первая и вторая сверху находятся на доске
                    if (j < Height - 2)
                    {
                        //Проверка существуют ли точки выше
                        if (allDots[i, j + 1] != null && allDots[i, j + 2] != null)
                        {
                            if (allDots[i, j + 1].tag == allDots[i, j].tag
                                && allDots[i, j + 2].tag == allDots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool SwitchAndCheck(int column, int row, Vector2 direction)
    {
        SwitchPieces(column, row, direction);

        if (CheckForMatches())
        {
            SwitchPieces(column, row, direction);
            return true;
        }
        SwitchPieces(column, row, direction);
        return false;
    }

    private bool isDeadlocker()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (i < Width - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.right))
                        {
                            return false;
                        }
                    }

                    if (j < Height - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    private void ShuffleBoard()
    {
        //Создаем список игровых объектов
        List<GameObject> newBoard = new List<GameObject>();

        //Добавление всех элементов в этот списон
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (allDots[i, j] != null)
                {
                    newBoard.Add(allDots[i, j]);
                }
            }
        }

        //Для каждого места на доске
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                //Если это место не должно быть пустым
                if (!blankSpaces[i, j] && !concreteTiles[i, j])
                {
                    //Выбираем случайное число
                    int pieceToUse = Random.Range(0, newBoard.Count);
                    
                    //Присваиваем новые значения
                    int maxIterations = 0;

                    while (MatchesAt(i, j, newBoard[pieceToUse]) && maxIterations < 100)
                    {
                        pieceToUse = Random.Range(0, newBoard.Count);
                        maxIterations++;
                    }
                    //Создаем контейнер для элемента
                    Dot piece = newBoard[pieceToUse].GetComponent<Dot>();
                    maxIterations = 0;

                    piece.column = i;
                    piece.row = j;
                    //Заполняем массив точек этой новой частью
                    allDots[i, j] = newBoard[pieceToUse];
                    //Удаляем из списка
                    newBoard.Remove(newBoard[pieceToUse]);
                }
            }
        }

        //Проверка на тупик
        if (isDeadlocker())
        {
            ShuffleBoard();
        }
    }

    private void RandomPopSound()
    {
        int clipToPlay = Random.Range(0, 2);

        if (clipToPlay == 0)
        {
            FindObjectOfType<AudioManager>().Play("Pop1");
        }
        else if (clipToPlay == 1)
        {
            FindObjectOfType<AudioManager>().Play("Pop2");
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Pop1");
        }
    }
}
