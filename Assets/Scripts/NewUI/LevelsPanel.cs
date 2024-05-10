using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsPanel : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private Button backButton;

    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;

    [SerializeField] private StartData data;

    private void OnEnable()
    {
        ButtonClickAction();
        SetButtonsColor();
    }

    private void ButtonClickAction()
    {
        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
            });
        }

        if (levelButtons[0] != null)
        {
            levelButtons[0].onClick.RemoveAllListeners();
            levelButtons[0].onClick.AddListener(() =>
            {
                if (data._isActive[0])
                {
                    data._selectedLevel = 0;
                    PlayerPrefs.SetInt("Current Level", data._selectedLevel);
                    SceneManager.LoadScene("Main");
                }
            });
        }
        
        if (levelButtons[1] != null)
        {
            levelButtons[1].onClick.RemoveAllListeners();
            levelButtons[1].onClick.AddListener(() =>
            {
                if (data._isActive[1])
                {
                    data._selectedLevel = 1;
                    PlayerPrefs.SetInt("Current Level", data._selectedLevel);
                    SceneManager.LoadScene("Main");
                }
            });
        }
        
        if (levelButtons[2] != null)
        {
            levelButtons[2].onClick.RemoveAllListeners();
            levelButtons[2].onClick.AddListener(() =>
            {
                if (data._isActive[2])
                {
                    data._selectedLevel = 2;
                    PlayerPrefs.SetInt("Current Level", data._selectedLevel);
                    SceneManager.LoadScene("Main");
                }
            });
        }
        
        if (levelButtons[3] != null)
        {
            levelButtons[3].onClick.RemoveAllListeners();
            levelButtons[3].onClick.AddListener(() =>
            {
                if (data._isActive[3])
                {
                    data._selectedLevel = 3;
                    PlayerPrefs.SetInt("Current Level", data._selectedLevel);
                    SceneManager.LoadScene("Main");
                }
            });
        }
        
        if (levelButtons[4] != null)
        {
            levelButtons[4].onClick.RemoveAllListeners();
            levelButtons[4].onClick.AddListener(() =>
            {
                if (data._isActive[4])
                {
                    data._selectedLevel = 4;
                    PlayerPrefs.SetInt("Current Level", data._selectedLevel);
                    SceneManager.LoadScene("Main");
                }
            });
        }
    }

    private void SetButtonsColor()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (data._isActive[i])
            {
                levelButtons[i].GetComponent<Image>().color = activeColor;
            }
            else
            {
                levelButtons[i].GetComponent<Image>().color = inactiveColor;
            }
        }
    }
}
