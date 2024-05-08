using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{
    [SerializeField]
    private Button backButton;
    [SerializeField] 
    private Button helpLevelButton;
    [SerializeField]
    private Button helpBoosterButton;

    [SerializeField] private GameObject buttonsGameObject;
    [SerializeField] private GameObject helpLevelGameObject;
    [SerializeField] private GameObject helpBoosterGameObject;

    private void Start()
    {
        ButtonClickAction();
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

        if (helpLevelButton != null)
        {
            helpLevelButton.onClick.RemoveAllListeners();
            helpLevelButton.onClick.AddListener(() =>
            {
                buttonsGameObject.SetActive(false);
                helpLevelGameObject.SetActive(true);
                BackButtonAction(buttonsGameObject, helpLevelGameObject);
            });
        }

        if (helpBoosterButton != null)
        {
            helpBoosterButton.onClick.RemoveAllListeners();
            helpBoosterButton.onClick.AddListener(() =>
            {
                buttonsGameObject.SetActive(false);
                helpBoosterGameObject.SetActive(true);
                BackButtonAction(buttonsGameObject, helpBoosterGameObject);
            });
        }
    }

    private void BackButtonAction(GameObject openPanel, GameObject closePanel)
    {
        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() =>
            {
                openPanel.SetActive(true);
                closePanel.SetActive(false);
                if (backButton != null)
                {
                    backButton.onClick.RemoveAllListeners();
                    backButton.onClick.AddListener(() =>
                    {
                        this.gameObject.SetActive(false);
                    });
                }
            });
        }
    }
}
