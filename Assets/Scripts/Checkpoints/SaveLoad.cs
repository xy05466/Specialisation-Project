using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public static class SaveLoad
{
    public static UnityAction OnSaveGame;
    public static UnityAction OnSaveStatus;
    public static UnityAction OnTempSaveGame;
    public static UnityAction OnSaveMap;
    public static UnityAction OnLoadScene;
    public static UnityAction<SaveData> OnLoadGame;
    public static UnityAction<List<CharacterStatus>> OnLoadParty;
    public static UnityAction<TempSaveData> OnTempLoadGame;
    public static UnityAction<MapSaveData> OnLoadMap;
    private static string directory = "/SaveData/";
    private static string fileName = "SaveGame.sav";
    private static string tempFileName = "ItemStatus.sav";
    private static string mapFileName;
    private static string statusFileName = "PartyStatus.sav";

    public static void Save(SaveData data,MapSaveData map, List<CharacterStatus> partyStatus)
    {
        OnSaveGame?.Invoke();
        OnSaveStatus?.Invoke();
        OnSaveMap?.Invoke();

        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir)) 
        {
            Directory.CreateDirectory(dir);
        }
        string json =JsonUtility.ToJson(data, true);
        File.WriteAllText(dir + fileName, json);

        string json1 = JsonUtility.ToJson(map, true);
        mapFileName = map.name;
        File.WriteAllText(dir + mapFileName +".sav", json1);

        string json2 = JsonConvert.SerializeObject(partyStatus, Formatting.None, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        File.WriteAllText(dir + statusFileName, json2);

        Debug.Log("Saving Game");
    }

    public static void TempSave(TempSaveData data, MapSaveData map)
    {
        OnTempSaveGame?.Invoke();
        OnSaveMap?.Invoke();
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(dir + tempFileName, json);

        string json1 = JsonUtility.ToJson(map, true);
        mapFileName = map.name;
        File.WriteAllText(dir + mapFileName + ".sav", json1);

        Debug.Log("Saving Game");
    }

    public static SaveData Load(string name)
    {
        string savepath = Application.persistentDataPath + directory;
        string partypath = Application.persistentDataPath + directory + statusFileName;
        string mappath = savepath + name + ".sav";
        string fullpath = savepath + fileName;

        SaveData data = new SaveData();
        MapSaveData map = new MapSaveData();
        List<CharacterStatus> party = new List<CharacterStatus>();

        if (File.Exists(fullpath))
        {
            string json = File.ReadAllText(fullpath);
            data = JsonUtility.FromJson<SaveData>(json);
            OnLoadGame?.Invoke(data);
            if (File.Exists (partypath))
            {
                string json2 = File.ReadAllText(partypath);
                party = JsonConvert.DeserializeObject<List<CharacterStatus>>(json2);
                OnLoadParty?.Invoke(party);
            }
           

            if (File.Exists(mappath))
            {
                string json1 = File.ReadAllText(mappath);
                map = JsonUtility.FromJson<MapSaveData>(json1);
                OnLoadMap?.Invoke(map);
            }
        }
        else
        {
            Debug.Log("save file does not exist");
        }
        return data;
    }

    public static TempSaveData TempLoad(string name)
    {
        string savepath = Application.persistentDataPath + directory;
        string fullpath = savepath + tempFileName;
        string mappath = savepath + name + ".sav";
        TempSaveData data = new TempSaveData();
        MapSaveData map = new MapSaveData();

        if (File.Exists(fullpath))
        {
            string json = File.ReadAllText(fullpath);
            data = JsonUtility.FromJson<TempSaveData>(json);
            //OnLoadStatus?.Invoke();
            OnTempLoadGame?.Invoke(data);
            if (File.Exists(mappath))
            {
                string json1 = File.ReadAllText(mappath);
                map = JsonUtility.FromJson<MapSaveData>(json1);
                OnLoadMap?.Invoke(map);
            }
        }
        else
        {
            Debug.Log("save file does not exist");
        }
        return data;
    }

    public static void DeleteSaveData()
    {
        PlayerPrefs.DeleteKey("LastPos");
        PlayerPrefs.DeleteKey("keynum");
        PlayerPrefs.DeleteKey("subkeynum");
        PlayerPrefs.DeleteKey("SceneName");
        PlayerPrefs.DeleteKey("didtutorial");

        System.IO.DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + directory);
        foreach (FileInfo fi in di.GetFiles())
        {
            File.Delete(fi.FullName);
        }

        //string fullPath = Application.persistentDataPath + directory + fileName;
        //if (File.Exists(fullPath))
        //{
        //    File.Delete(fullPath);
        //}
        //DeleteTempSaveData();
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
