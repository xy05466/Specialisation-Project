using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Eflatun.SceneReference;
public class SceneManagement : MonoBehaviour
{
    private static SceneManagement instance = null;
    public Animator transition;
    public CanvasGroup canvasGroup;
    private List<string> sceneHistory = new List<string>();

    public static SceneManagement Instance { get {
            if (instance == null)
            {
                GameObject newGO = Instantiate(Resources.Load("SceneManager") as GameObject, null);
                newGO.name = "SceneManager";
                newGO.SetActive(true);
                DontDestroyOnLoad(newGO);
            }
            return instance;
        } }

    private AsyncOperation asyncHandler = null;
    private Coroutine loadCoroutine = null;
    
    public bool isLoading { get; private set; }
    public bool isLoadingEffect { get; private set; }

    private System.Action onSceneLoaded;

    //public Volume dissolveVolume;

    public void OnSceneLoaded(System.Action onSceneLoad)
    {
        if (isLoading)
            onSceneLoaded += onSceneLoad;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }   
        DontDestroyOnLoad(this);
        instance = this;
        //transition.enabled = false;
        //canvasGroup.alpha = 0.0f;
        //loadingText.gameObject.SetActive(false);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        LoadScene(SceneManager.GetActiveScene().name);
    //    }
    //}

    public void LoadScene(string name)
    {
        if (loadCoroutine == null)
            loadCoroutine = StartCoroutine(LoadSceneRoutine(name));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        sceneHistory.Add(sceneName);
        transition.SetBool("Start", true);
        isLoading = true;
        isLoadingEffect = true;
        Time.timeScale = 0;

        asyncHandler = SceneManager.LoadSceneAsync(sceneName);

        if (asyncHandler == null)
            yield break;

        //When the async progress is not done
        while (!asyncHandler.isDone)
        {
            Debug.Log(string.Format("Load Scene {0}  Progress: {1:P2}", sceneName, asyncHandler.progress));
            yield return null;
        }
        asyncHandler = null;
        onSceneLoaded?.Invoke();
        onSceneLoaded = null;
        isLoading = false;
        loadCoroutine = null;

        //Default set game time scale to 1 when a new scene is loaded
        Time.timeScale = 1;
        isLoadingEffect = false;
        transition.SetBool("Start", false);
    }

    public string GetPreviousScene()
    {
        if (sceneHistory.Count >= 2)  //Checking that we have actually switched scenes enough to go back to a previous scene
        {
            sceneHistory.RemoveAt(sceneHistory.Count - 1);
            return sceneHistory[sceneHistory.Count - 1].ToString();
        }
        else
        {
            return "";
        }
    }


    public void Exit()
    {
        Application.Quit();
    }

    public void OnSceneLoadSetTimeScale(float timeScale)
    {
        onSceneLoaded += () => Time.timeScale = timeScale;
    }

    public string GetActiveSceneName() => SceneManager.GetActiveScene().name;
}