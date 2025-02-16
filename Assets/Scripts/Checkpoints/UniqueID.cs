using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
[ExecuteInEditMode]
public class UniqueID : MonoBehaviour
{
    [ReadOnly, SerializeField] private string id = Guid.NewGuid().ToString();
    [SerializeField] private static SerializableDict<string, GameObject> idDatabase = new SerializableDict<string, GameObject>();

    public string ID => id;
    // Start is called before the first frame update

    private void Start()
    {
        if (idDatabase.ContainsKey(id))
        {
            Generate();
        }
        else
        {
            idDatabase.Add(id, this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (idDatabase.ContainsKey(id))
        {
            idDatabase.Remove(id);
        }
    }

    public void Generate()
    {
        id = Guid.NewGuid().ToString();
        idDatabase.Add(id,this.gameObject);
    }


}
