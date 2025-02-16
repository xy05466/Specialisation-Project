using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject manaTarget;
    public GameObject manaDebt;
    public GameObject background;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("didtutorial", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (manaTarget.activeSelf && Input.GetKeyDown(KeyCode.K))
        {
            manaTarget.SetActive(false);
            StartCoroutine(SetRedoActive());
        }
    }

    IEnumerator SetRedoActive()
    {
        yield return new WaitForSeconds(3.5f);
        manaDebt.SetActive(true);
        background.SetActive(true);
    }
}
