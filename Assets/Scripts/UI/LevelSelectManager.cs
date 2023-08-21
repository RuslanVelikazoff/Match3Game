using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject currentPanel;

    public int page;
    public int currentLevel = 0;

    private StartData startData;

    private void Start()
    {

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        if (startData != null)
        {
            for (int i = 0; i < startData._isActive.Length; i++)
            {
                if (startData._isActive[i])
                {
                    currentLevel = i;
                }
            }
        }

        page = (int)Mathf.Floor(currentLevel / 9); //THIS
        currentPanel = panels[page];
        panels[page].SetActive(true);
    }

    public void PageRight()
    {
        if (page < panels.Length - 1)
        {
            currentPanel.SetActive(false);
            page++;
            currentPanel = panels[page];
            currentPanel.SetActive(true);
        }
    }

    public void PageLeft()
    {
        if (page > 0)
        {
            currentPanel.SetActive(false);
            page--;
            currentPanel = panels[page];
            currentPanel.SetActive(true);
        }
    }
}
