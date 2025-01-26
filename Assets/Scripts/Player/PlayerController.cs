using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance = null;
    private PlayerMovementController playerMovementController;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject.transform.parent.gameObject);
            return;
        }
        Instance = this;
        if (transform.parent == null)
            DontDestroyOnLoad(this);
        playerMovementController = GetComponent<PlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        //playerMovementController.UpdateMovement();
    }

    public IEnumerator LoadLevel(string nextLevel, Vector3 nextSpawnPos)
    {
        SceneManagement.Instance.LoadScene(nextLevel);

        while (SceneManagement.Instance.isLoading)
        {
            Debug.Log("ISLOADING");
            yield return null;
        }

        transform.position = nextSpawnPos;
    }
}
