using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class SubKey : MonoBehaviour
{
    private string id;
    // Start is called before the first frame update
    void Awake()
    {
        id = GetComponent<UniqueID>().ID;
        SaveLoad.OnLoadGame += LoadGame;
        SaveLoad.OnTempLoadGame += LoadGame;
    }
    private void LoadGame(SaveData saveData)
    {
        if (saveData.collectedItems.Contains(id))
        {
            Debug.Log(this.gameObject.name);
            Destroy(this.gameObject);


        }
    }

    private void LoadGame(TempSaveData saveData)
    {
        if (saveData.collectedItems.Contains(id))
        {
            Debug.Log(this.gameObject.name);
            Destroy(this.gameObject);


        }
    }
    private void OnDestroy()
    {
        PlayerPrefs.SetInt("subkeynum", PlayerPrefs.GetInt("subkeynum", 0) + 1);
        SaveGameManager.data.collectedItems.Add(id);
        SaveGameManager.tempData.collectedItems.Add(id);
        SaveLoad.OnLoadGame -= LoadGame;
        SaveLoad.OnTempLoadGame -= LoadGame;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
