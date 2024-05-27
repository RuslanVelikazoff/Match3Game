using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGamesPanel : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button miniGame1Button;

    private void OnEnable()
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

        if (miniGame1Button != null)
        {
            miniGame1Button.onClick.RemoveAllListeners();
            miniGame1Button.onClick.AddListener(() =>
            {
                Screen.orientation = ScreenOrientation.LandscapeRight;
                SceneManager.LoadScene("MiniGame1");
            });
        }
    }
}
