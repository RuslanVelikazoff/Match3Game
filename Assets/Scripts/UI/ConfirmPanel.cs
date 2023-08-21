using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfirmPanel : MonoBehaviour
{
    [Header("Информация про уровень")]
    public string levelToLoad;
    public int level;
    private int starsActive;
    private int[] starsDB;
    private int highScore;
    private int[] scoreDB;

    [Header("UI элементы")]
    public Image[] stars;
    public Text highScoreText;
    public Text starText;

    private const string saveKey = "mainSave";

    private void OnEnable()
    {
        ActivateStars();
        SetText();
    }

    void SetText()
    {
        highScoreText.text = "" + highScore;
        starText.text = "" + starsActive + "/3";
    }

    void ActivateStars()
    {
        for (int i = 0; i < starsActive; i++)
        {
            stars[i].enabled = true;
        }
    }

    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }

    public void Play()
    {
        PlayerPrefs.SetInt("Current Level", level - 1);
        SceneManager.LoadScene(levelToLoad);
    }
}
