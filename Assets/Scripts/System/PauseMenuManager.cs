using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject chestMenu;
    public GameObject healMenu;
    public GameObject normalStatusMenu;
    public GameObject battleStatusMenu;
    public GameObject saveLoad;
    //public GameObject status;
    //private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Room"))
        {
            saveLoad.SetActive(false);
        }
        else if (scene.name.Contains("SampleScene"))
        {
            saveLoad.SetActive(true);
        }
        Debug.Log("OnSceneLoaded: " + scene.name);
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            chestMenu.SetActive(!chestMenu.activeSelf);
            normalStatusMenu.SetActive(!normalStatusMenu.activeSelf);
            battleStatusMenu.SetActive(!battleStatusMenu.activeSelf);
        }
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            chestMenu.SetActive(false);
            healMenu.SetActive(false);
            normalStatusMenu.SetActive(false);
            battleStatusMenu.SetActive(false);
        }
    }
}
