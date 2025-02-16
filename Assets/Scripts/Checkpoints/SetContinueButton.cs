using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetContinueButton : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;
    // Start is called before the first frame update
    void Start()
    {
        string sceneName = PlayerPrefs.GetString("SceneName");
        if (string.IsNullOrWhiteSpace(sceneName))
            continueButton.interactable = false;
    }

}
