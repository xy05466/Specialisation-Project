using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int posX;
    private int posZ;

    public bool isPlayerOn;

    public bool discovered;
    private MapManager mapManager;

    // Start is called before the first frame update
    void Start()
    {
        PostOffice.GetInstance().Subscribe(this.gameObject);
        mapManager = FindObjectOfType<MapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !isPlayerOn)
        {
            mapManager.CheckTile(new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.z));
            isPlayerOn = true;
            Debug.Log(this.name);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerOn = false;
            //Debug.Log(this.name);
        }
    }
}
