using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuPanel : MonoBehaviour
{
    [SerializeField] 
    private Button backButton;
    [SerializeField]
    private Button musicButton;
    [SerializeField] 
    private Button soundButton;
    [SerializeField]
    private Button helpButton;

    [SerializeField] 
    private GameObject helpPanel;
    
    [SerializeField] 
    private Sprite onSprite;
    [SerializeField]
    private Sprite offSprite;

    private void OnEnable()
    {
        CheckMusicVolume();
        CheckSoundVolume();
        
        ButtonClickAction();
    }

    private void ButtonClickAction()
    {
        if (musicButton != null)
        {
            musicButton.onClick.RemoveAllListeners();
            musicButton.onClick.AddListener(() =>
            {
                MusicButton();
            });
        }

        if (soundButton != null)
        {
            soundButton.onClick.RemoveAllListeners();
            soundButton.onClick.AddListener(() =>
            {
                SoundButton();
            });
        }

        if (helpButton != null)
        {
            helpButton.onClick.RemoveAllListeners();
            helpButton.onClick.AddListener(() =>
            {
                helpPanel.SetActive(true);
            });
        }

        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
            });
        }
    }

    private void CheckSoundVolume()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                soundButton.GetComponent<Image>().sprite = offSprite;
            }
            else
            {
                soundButton.GetComponent<Image>().sprite = onSprite;
            }
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = onSprite;
        }
    }

    private void CheckMusicVolume()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                musicButton.GetComponent<Image>().sprite = offSprite;
            }
            else
            {
                musicButton.GetComponent<Image>().sprite = onSprite;
            }
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = onSprite;
        }
    }

    private void SoundButton()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                PlayerPrefs.SetInt("Sound", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Sound", 0);
            }
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
        
        CheckSoundVolume();
    }

    private void MusicButton()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                PlayerPrefs.SetInt("Music", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Music", 0);
            }
        }
        else
        {
            PlayerPrefs.SetInt("Music", 1);
        }
        
        CheckMusicVolume();
    }
}
