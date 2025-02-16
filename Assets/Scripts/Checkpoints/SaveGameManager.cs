using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGameManager : MonoBehaviour
{
    public static SaveData data;
    public static TempSaveData tempData;
    public static MapSaveData mapData;
    public static List<CharacterStatus> partyStatusData;
    public GameObject saveLoad;
    private SceneManagement sceneManagement;


    private void Start()
    {
        //SaveLoad.OnLoadScene += StartLoadScene;
    }
    private void OnEnable()
    {
        sceneManagement = SceneManagement.Instance;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    IEnumerator DisableSaveLoad()
    {
        yield return new WaitForSeconds(3);
        this.SetActive(false);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Room") || scene.name.Contains("BattleArena"))
        {
            saveLoad.SetActive(false);
        }
        Debug.Log("OnSceneLoaded: " + scene.name);
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        data = new SaveData();
        tempData = new TempSaveData();
        mapData = new MapSaveData();
        partyStatusData = new List<CharacterStatus>();

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
        var map = mapData;
        var partyStatus = partyStatusData;
        PlayerPrefs.SetString("LastPos", transform.parent.position.ToString());
        PlayerPrefs.SetString("SceneName", SceneManager.GetActiveScene().name);
        SaveLoad.Save(saveData, map, partyStatus);
    }

    public void TempSaveData()
    {
        var tempdata = tempData;
        var map = mapData;  
        SaveLoad.TempSave(tempdata, map);
    }

    public void LoadData(SaveData _data)
    {
        data = _data;
    }

    public void StartLoadScene(Vector3 nextSpawnPos, string sceneName)
    {
        StartCoroutine(LoadScene(nextSpawnPos, sceneName));
    }

    IEnumerator LoadScene(Vector3 nextSpawnPos, string sceneName)
    {
        while (sceneManagement.isLoading) { yield return null; }

        sceneManagement.LoadScene(sceneName);
        while (sceneManagement.isLoading)
        {
            yield return null;
        }
        transform.parent.position = nextSpawnPos;
        Debug.Log("scene finished loading");
    }

    public void TempLoadData(TempSaveData _data)
    {
        tempData = _data;
    }

    public void StartTryLoadData()
    {
        StartCoroutine(TryLoadData());
    }

    public IEnumerator TryLoadData()
    {
        while (sceneManagement.isLoading) { yield return null; }

        sceneManagement.LoadScene(PlayerPrefs.GetString("SceneName"));
        while (sceneManagement.isLoading)
        {
            yield return null;
        }
        Debug.Log(PlayerPrefs.GetString("LastPos"));
        transform.parent.position = StringToVector3(PlayerPrefs.GetString("LastPos"));
        Debug.Log(transform.parent.position + "data finished loading");
        SaveLoad.Load(SceneManager.GetActiveScene().name);
    }

    public void TryTempLoadData()
    {
        SaveLoad.TempLoad(SceneManager.GetActiveScene().name);
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
