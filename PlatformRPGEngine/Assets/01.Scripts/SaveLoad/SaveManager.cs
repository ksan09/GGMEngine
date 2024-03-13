using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoSingleton<SaveManager>
{
    [SerializeField] private string fileName;
    private GameData _gameData;

    private List<ISaveManager> _saveManagerList;
    private FileDataHandler _fileDataHandler;

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void Start()
    {
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        _saveManagerList = FindAllSaveManagers();

        LoadGame();
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        _gameData = _fileDataHandler.Load();
        if(_gameData == null)
        {
            Debug.Log("No save data found");
            NewGame();
        }

        foreach(ISaveManager manager in _saveManagerList)
        {
            manager.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        foreach(ISaveManager manager in _saveManagerList)
        {
            manager.SaveData(ref _gameData);
        }
        _fileDataHandler.Save(_gameData);
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        return FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>().ToList();
    }

    [ContextMenu("Delete save file")]
    public void DeleteSaveData()
    {
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        _fileDataHandler.DeleteSaveData();
    }
}
