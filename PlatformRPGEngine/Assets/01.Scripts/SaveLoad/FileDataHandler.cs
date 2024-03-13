
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileDataHandler
{
    private string _directoryPath = "";
    private string _fileName = "";

    public FileDataHandler(string directoryPath, string fileName)
    {
        _directoryPath = directoryPath;
        _fileName = fileName;
    }

    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(_directoryPath, _fileName);

        try
        {
            Directory.CreateDirectory(_directoryPath);
            string dataToStore = JsonUtility.ToJson(gameData, true);

            using (FileStream writeStream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(writeStream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception ex)
        {
            Debug.LogError($"Error on trying to save data to file {fullPath} \n {ex.Message}");
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(_directoryPath, _fileName);
        GameData loadedData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream readStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(readStream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            } catch(Exception ex)
            {
                Debug.LogError($"Error on trying to load data to file {fullPath} \n {ex.Message}");
            }
        }

        return loadedData;
    }

    public void DeleteSaveData()
    {
        string fullPath = Path.Combine(_directoryPath, _fileName);

        if(File.Exists(fullPath))
        {
            try
            {
                File.Delete(fullPath);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error on trying to delete data to file {fullPath} \n {ex.Message}");
            }
        }
    }
}
