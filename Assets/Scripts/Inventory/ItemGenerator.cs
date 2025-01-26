using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> items;

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, items.Count);
        Instantiate(items[rand], gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
