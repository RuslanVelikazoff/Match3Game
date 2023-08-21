using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Image soundButton;
    public Image musicButton;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    public bool paused = false;

    private Board board;

    private void Start()
    {
        CheckSoundVolume();
        CheckMusicVolume();

        pausePanel.SetActive(false);
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
    }

    private void Update()
    {
        if (paused && !pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(true);
            board.currentState = GameState.pause;
        }
        if (!paused && pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            board.currentState = GameState.move;
        }
    }

    private void CheckSoundVolume()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                soundButton.sprite = musicOffSprite;
            }
            else
            {
                soundButton.sprite = musicOnSprite;
            }
        }
        else
        {
            soundButton.sprite = musicOnSprite;
        }
    }

    private void CheckMusicVolume()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                musicButton.sprite = musicOffSprite;
            }
            else
            {
                musicButton.sprite = musicOnSprite;
            }
        }
        else
        {
            musicButton.sprite = musicOnSprite;
        }
    }

    public void SoundButton()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                soundButton.sprite = musicOnSprite;
                PlayerPrefs.SetInt("Sound", 1);
            }
            else
            {
                soundButton.sprite = musicOffSprite;
                PlayerPrefs.SetInt("Sound", 0);
            }
        }
        else
        {
            soundButton.sprite = musicOffSprite;
            PlayerPrefs.SetInt("Sound", 1);
        }
    }

    public void MusicButton()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                musicButton.sprite = musicOnSprite;
                PlayerPrefs.SetInt("Music", 1);
            }
            else
            {
                musicButton.sprite = musicOffSprite;
                PlayerPrefs.SetInt("Music", 0);
            }
        }
        else
        {
            musicButton.sprite = musicOffSprite;
            PlayerPrefs.SetInt("Music", 1);
        }
    }

    public void PauseGame()
    {
        paused = !paused;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
