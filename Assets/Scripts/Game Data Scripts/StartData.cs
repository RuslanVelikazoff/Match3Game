using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartData : MonoBehaviour
{
    public bool[] _isActive;

    private const string saveKey = "mainSave";

    private void Start()
    {
        Load();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnDisable()
    {
        Save();
    }

    public void Load()
    {
        var data = SaveManager.Load<SaveData.GameData>(saveKey);

        _isActive = data.isActive;

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
            isActive = _isActive
        };

        return data;
    }
}
