using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class BackToSplash : MonoBehaviour
{
    private string sceneToLoad = "Menu";

    private Board board;

    private bool[] nextIsActive;
    private const string saveKey = "mainSave";

    private void Start()
    {
        Load();
        board = FindObjectOfType<Board>();
        sceneToLoad = "Menu";
    }

    public void WinOK()
    {
        nextIsActive[board.level + 1] = true;
        Save();
        SceneManager.LoadScene(sceneToLoad);
    }


    public void LoseOK()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void Load()
    {
        var data = SaveManager.Load<SaveData.GameData>(saveKey);

        nextIsActive = data.isActive;

        Debug.Log("Данные загружены");
    }

    public void Save()
    {
        SaveManager.Save(saveKey, GetSaveSnapshot());
        PlayerPrefs.Save();
        Debug.Log("Данные сохранены");
    }

    public SaveData.GameData GetSaveSnapshot()
    {
        var data = new SaveData.GameData()
        {
            isActive = nextIsActive
        };

        return data;
    }
}
