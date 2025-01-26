using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public static class SaveLoad
{
    public static UnityAction OnSaveGame;
    public static UnityAction OnTempSaveGame;
    public static UnityAction<SaveData> OnLoadGame;
    public static UnityAction<TempSaveData> OnTempLoadGame;
    private static string directory = "/SaveData/";
    private static string fileName = "SaveGame.sav";
    private static string tempFileName = "ItemStatus.sav";

    public static void Save(SaveData data)
    {
        OnSaveGame?.Invoke();
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir)) 
        {
            Directory.CreateDirectory(dir);
        }
        string json =JsonUtility.ToJson(data, true);
        File.WriteAllText(dir + fileName, json);

        

        Debug.Log("Saving Game");
    }

    public static void TempSave(TempSaveData data)
    {
        OnTempSaveGame?.Invoke();
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(dir + tempFileName, json);

        Debug.Log("Saving Game");
    }

    public static SaveData Load()
    {
        string fullpath = Application.persistentDataPath + directory + fileName;
        SaveData data = new SaveData();

        if (File.Exists(fullpath))
        {
            string json = File.ReadAllText(fullpath);
            data = JsonUtility.FromJson<SaveData>(json);
                OnLoadGame?.Invoke(data);
        }
        else
        {
            Debug.Log("save file does not exist");
        }
        return data;
    }

    public static TempSaveData TempLoad()
    {
        string fullpath = Application.persistentDataPath + directory + tempFileName;
        TempSaveData data = new TempSaveData();

        if (File.Exists(fullpath))
        {
            string json = File.ReadAllText(fullpath);
            data = JsonUtility.FromJson<TempSaveData>(json);
            //OnLoadStatus?.Invoke();
            OnTempLoadGame?.Invoke(data);
        }
        else
        {
            Debug.Log("save file does not exist");
        }
        return data;
    }

    public static void DeleteSaveData()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public static void DeleteTempSaveData()
    {
        string fullPath = Application.persistentDataPath + directory + tempFileName;
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
