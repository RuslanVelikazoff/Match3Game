using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button[] levelButtons;
    public Text[] buttonText;

    public Sprite activeSprite;
    public Sprite lockedSprite;

    public bool[] isActive;

    public string levelToLoad = "Main";
    private const string saveKey = "mainSave";

    private void Start()
    {
        Load();
        Initialized();
    }

    void ActivatedLevel()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (isActive[i])
            {
                levelButtons[i].GetComponent<Image>().sprite = activeSprite;
                levelButtons[i].enabled = true;
                buttonText[i].enabled = true;
            }
            else
            {
                levelButtons[i].GetComponent<Image>().sprite = lockedSprite;
                levelButtons[i].enabled = false;
                buttonText[i].enabled = false;
            }
        }
    }

    void Initialized()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            ButtonFunc(levelButtons[i], i);
        }
    }

    void ButtonFunc(Button button, int level)
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                PlayerPrefs.SetInt("Current Level", level);
                UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoad);
            });
        }
    }

    private void Load()
    {
        var data = SaveManager.Load<SaveData.GameData>(saveKey);
        isActive = data.isActive;

        ActivatedLevel();
    }
}
