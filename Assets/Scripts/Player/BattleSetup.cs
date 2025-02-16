using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSetup : MonoBehaviour
{
    public GameObject invUI;
    public GameObject pauseUI;
    public GameObject saveLoadUI;
    private GameObject model;
    //public SaveGameManager saveGameManager;
    // Start is called before the first frame update

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
        model = transform.GetChild(0).gameObject;
    }

    // called third
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("BattleArena"))
        {
            invUI.SetActive(false);
            pauseUI.SetActive(false);
            saveLoadUI.SetActive(false);
            model.transform.localScale = new Vector3(2f, 2f, 2f);
        }
        else
        {
            model.transform.localScale = new Vector3(1f, 1f, 1f);
        }

    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
