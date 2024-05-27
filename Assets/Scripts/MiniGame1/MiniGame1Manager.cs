using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGame1Manager : MonoBehaviour
{
    public static MiniGame1Manager Instance;


    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject obstaclePrefab;

    [SerializeField]
    Text _scoreText, _endScoreText, _highScoreText;

    private int score,highScore;
    private bool hasGameFinished;
    private const string HIGHSCORE = "HIGHSCORE";


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        startPanel.SetActive(true);
        helpPanel.SetActive(false);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        player.SetActive(false);

        score = 0;
        hasGameFinished = false;
    }

    public void StartHelp()
    {
        startPanel.SetActive(false);
        helpPanel.SetActive(true);
    }

    public void UpdateScore(int v)
    {
        score += v;
        _scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        hasGameFinished = true;

        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        foreach (var item in obstacles)
        {
            item.gameObject.SetActive(false);
        }

        highScore = PlayerPrefs.HasKey(HIGHSCORE) ? PlayerPrefs.GetInt(HIGHSCORE) : 0;
        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HIGHSCORE, highScore);
        }
        _highScoreText.text = "HIGHSCORE " + highScore.ToString();
        _endScoreText.text = "SCORE " + score.ToString();

        gameOverPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void StartGame()
    {
        helpPanel.SetActive(false);
        gamePanel.SetActive(true);
        player.SetActive(true);

        StartCoroutine(Spawner());

    }

    public void GameRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame1");
    }

    public void GameQuit()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    IEnumerator Spawner()
    {
        while(!hasGameFinished)
        {
            Instantiate(obstaclePrefab, Vector3.zero, obstaclePrefab.transform.rotation);
            yield return new WaitForSeconds(2f);
        }
    }
}
