using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header("Открыт ли уровень")]
    public bool active;
    private bool[] isActiveLevel;
    public Sprite activeSprite;
    public Sprite lockedSprite;
    private Image buttonImage;
    private Button myButton;

    [Header("UI объекты")]
    public Text levelText;
    public GameObject confirmPanel;
    public Image[] stars;
    private int activeStars;
    private int[] starsActive;

    [Header("Номер уровня")]
    public int level;

    private const string saveKey = "mainSave";


    private void Start()
    {
        Load();

        buttonImage = GetComponent<Image>();
        myButton = GetComponent<Button>();

        ActiveLevel();
        ActivateStars();
        ShowLevel();
        DecideSprite();
    }

    void ActiveLevel()
    {
        if (level == 1)
        {
            active = true;
        }
    }

    void ActivateStars()
    {
        for (int i = 0; i < activeStars; i++)
        {
            stars[i].enabled = true;
        }
    }

    void DecideSprite()
    {
        if (active)
        {
            buttonImage.sprite = activeSprite;
            myButton.enabled = true;
            levelText.enabled = true;
        }
        else
        {
            buttonImage.sprite = lockedSprite;
            myButton.enabled = false;
            levelText.enabled = false;
        }
    }

    void ShowLevel()
    {
        levelText.text = "" + level;
    }

    public void ConfirmPanel(int level)
    {
        confirmPanel.GetComponent<ConfirmPanel>().level = level;
        confirmPanel.SetActive(true);
    }

    private void Load()
    {
        var data = SaveManager.Load<SaveData.GameData>(saveKey);
        isActiveLevel = data.isActive;

        active = isActiveLevel[level - 1];
    }
}
