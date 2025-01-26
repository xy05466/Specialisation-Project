using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGameManager : MonoBehaviour
{
    public static SaveData data;
    public static TempSaveData tempData;
    private bool isLoading;

    private void Awake()
    {
        data = new SaveData();
        tempData = new TempSaveData();
        SaveLoad.OnLoadGame += LoadData;
        SaveLoad.OnTempLoadGame += TempLoadData;
    }

    public void DeleteData()
    {
        SaveLoad.DeleteSaveData();
    }

    public void SaveData()
    {
        var saveData = data;
        PlayerPrefs.SetString("CheckpointPos", transform.position.ToString());
        PlayerPrefs.SetString("SceneName", SceneManager.GetActiveScene().name);
        SaveLoad.Save(saveData);
    }

    public void TempSaveData()
    {
        var tempdata = tempData;
        SaveLoad.TempSave(tempdata);
    }

    public void StartLoadSceneAsync()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        isLoading = true;
        AsyncOperation operation = SceneManager.LoadSceneAsync(PlayerPrefs.GetString("SceneName"));
        while (!operation.isDone)
        {
            Debug.Log(operation.progress);
            yield return null;
        }
        isLoading = false;
    }

    public void LoadData(SaveData _data)
    {
        StartCoroutine(StartLoadData(data));
    }

    IEnumerator StartLoadData(SaveData _data)
    {
        StartLoadSceneAsync();
        while (isLoading)
        {
            yield return null;
        }
        transform.position = StringToVector3(PlayerPrefs.GetString("CheckpointPos"));
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        data = _data;
    }

    public void TempLoadData(TempSaveData _data)
    {
        tempData = _data;
    }

    public void TryLoadData()
    {
        SaveLoad.Load();
    }

    public void TryTempLoadData()
    {
        SaveLoad.TempLoad();
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
}
