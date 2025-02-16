using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SubKeyDoor : MonoBehaviour, IInteractable
{
    public GameObject interactionImage;
    public TMP_Text keyText;
    private GameObject playerObject;
    private HashSet<GameObject> objectsInsideSphere = new HashSet<GameObject>();
    private Vector3 prevPlayerPos;
    private int subKeyNum;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    private Animator animator;
    public readonly int openFront = Animator.StringToHash("RegularDoorOpenFront");
    public readonly int closeFront = Animator.StringToHash("RegularDoorCloseFront");
    public readonly int openBack = Animator.StringToHash("RegularDoorOpenBack");
    public readonly int closeBack = Animator.StringToHash("RegularDoorCloseBack");
    public readonly int openOneWay = Animator.StringToHash("RegularDoorOpenFront");

    private bool isOpened;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        HashSet<GameObject> currentObjects = new HashSet<GameObject>();
        var colliders = Physics.OverlapSphere(transform.position, 1.5f);
        for (int i = 0; i < colliders.Length; i++)
        {
            currentObjects.Add(colliders[i].gameObject);
            if (colliders[i].gameObject.tag == "NormalPresence" && !interactionImage.activeSelf)
            {
                interactionImage.SetActive(true);
                subKeyNum = PlayerPrefs.GetInt("subkeynum", 0);
                keyText.text = $"{subKeyNum}/1 Sub Key";
                playerObject = colliders[i].gameObject;

            }
        }

        foreach (var obj in objectsInsideSphere)
        {
            if (!currentObjects.Contains(obj))
            {
                if (playerObject != null)
                {
                    interactionImage.SetActive(false);
                }


            }
        }
        objectsInsideSphere = currentObjects;
    }

    public void EndInteraction()
    {
        Vector3 directionToTarget = prevPlayerPos - transform.position;
        float dotProductLR = Vector3.Dot(transform.right, directionToTarget);
        float dotProductUD = Vector3.Dot(transform.forward, directionToTarget);

        if (dotProductLR > 0)
        {
            Debug.Log("Target is to the right");
            animator.CrossFade(closeFront, 0.1f);
        }
        else if (dotProductLR < 0)
        {
            Debug.Log("Target is to the left");
            animator.CrossFade(closeBack, 0.1f);
        }
        if (dotProductUD > 0)
        {
            animator.CrossFade(closeBack, 0.1f);
            Debug.Log("Target is in front");
        }
        else if (dotProductUD < 0)
        {
            animator.CrossFade(closeFront, 0.1f);
            Debug.Log("Target is behind");

        }
        isOpened = false;
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        if (!isOpened && PlayerPrefs.GetInt("subkeynum", 0) >= 1)
        {
            animator.enabled = true;
            Vector3 directionToTarget = interactor.transform.position - transform.position;
            prevPlayerPos = interactor.transform.position;

            if (transform.rotation.z == 0)
            {
                float dotProductUD = Vector3.Dot(transform.forward, directionToTarget);
                if (dotProductUD > 0)
                {
                    animator.CrossFade(openBack, 0.1f);
                    Debug.Log("Target is in front");
                }
                else if (dotProductUD < 0)
                {
                    animator.CrossFade(openFront, 0.1f);
                    Debug.Log("Target is behind");

                }
            }
            else if (transform.rotation.z == 90)
            {
                float dotProductLR = Vector3.Dot(transform.right, directionToTarget);
                if (dotProductLR > 0)
                {
                    Debug.Log("Target is to the right");
                    animator.CrossFade(openFront, 0.1f);
                }
                else if (dotProductLR < 0)
                {
                    Debug.Log("Target is to the left");
                    animator.CrossFade(openBack, 0.1f);
                }
            }

            isOpened = true;
        }
        interactSuccessful = true;
    }
}
