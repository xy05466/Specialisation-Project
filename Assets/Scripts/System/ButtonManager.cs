using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button newGameButton;
    public Button continueButton;
    public Button menuButton;
    public Button exitButton;
    private SceneManagement sceneManagement;
    // Start is called before the first frame update
    void Start()
    {
        sceneManagement = GameObject.FindWithTag("SceneManager").GetComponent<SceneManagement>();
        if (newGameButton != null)
        {
            newGameButton.onClick.AddListener(LoadOverworldDungeon);
        }
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(LoadOverworldDungeon);
        }
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(Exit);
        }
        if (menuButton != null )
        {
            menuButton.onClick.AddListener(LoadStartScene);
        }

    }

    private void LoadOverworldDungeon()
    {
        SceneManagement.Instance.LoadScene("OverworldDungeon");
    }

    private void LoadStartScene()
    {
        SceneManagement.Instance.LoadScene("Start");
    }

    private void Exit()
    {
        SceneManagement.Instance.Exit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
