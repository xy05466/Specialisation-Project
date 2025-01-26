using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSetup : MonoBehaviour
{
    public GameObject statusDisplay;
    //public SaveGameManager saveGameManager;
    // Start is called before the first frame update

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called third
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("BattleArena"))
        {
            statusDisplay.SetActive(true);
        }
        //else
        //{
        //    statusDisplay.SetActive(false);
        //}
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
