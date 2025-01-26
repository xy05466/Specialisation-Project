using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBomb : MonoBehaviour, IUsableItems
{
    public Action EndUse()
    {
        return delegate
        {
            Debug.Log("end use");
        };
    }

    public Action UseItem(string name)
    {
        return delegate
        {
            SceneLoader.instance.LoadLevel(PlayerPrefs.GetString("SceneName"));
        };
    }
}
